USE [POS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[NewOrderTransaction]
    @User NVARCHAR(25),
    @Products NVARCHAR(MAX),
    @Discount DECIMAL(18, 2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1
                   FROM   Users
                   WHERE  Username = @User)
    BEGIN
        THROW 51000, 'User does not exist!', 1;
    END 

    IF @Discount IS NOT NULL
    BEGIN
	   DECLARE @DiscountValid DECIMAL(18, 2);
       SELECT @DiscountValid = DiscountAmount
       FROM Discounts WITH (NOLOCK)
       WHERE UserRoleID = (SELECT UserRoleID FROM Users WHERE Username = @User)
       AND DiscountAmount = @Discount;
    END

    IF @DiscountValid IS NULL AND @Discount IS NOT NULL
    BEGIN
        THROW 50002, 'Invalid discount amount, please try again.', 1;
    END

    CREATE TABLE #ProductQuantities
    (
        ProductID INT,
        ProductQuantity INT
    );

    INSERT INTO #ProductQuantities (ProductID, ProductQuantity)
    SELECT JSON_VALUE(p.Value, '$.ProductID') AS ProductID,
           JSON_VALUE(p.Value, '$.ProductQuantity') AS ProductQuantity
    FROM OPENJSON(@Products) AS p;

    IF EXISTS (
        SELECT 1
        FROM #ProductQuantities pq
        INNER JOIN Products p ON pq.ProductID = p.ProductID
        WHERE pq.ProductQuantity > p.ProductStock
    )
    BEGIN
        DROP TABLE #ProductQuantities;
        THROW 50001, 'Product quantity exceeds its stock, please update your cart.', 1;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OrderID NVARCHAR(50) = NEWID();
        DECLARE @OrderSubTotal DECIMAL(18, 2) = 0;
        DECLARE @OrderTotal DECIMAL(18, 2) = 0;
        DECLARE @OrderDate DATETIME = GETDATE();
        
        SELECT @OrderSubTotal = SUM(pq.ProductQuantity * p.ProductPrice)
        FROM #ProductQuantities pq
        INNER JOIN Products p ON pq.ProductID = p.ProductID;

        IF @Discount IS NOT NULL
        BEGIN
            SET @OrderTotal = @OrderSubTotal - (@OrderSubTotal * @Discount);
        END
        ELSE
        BEGIN
            SET @OrderTotal = @OrderSubTotal;
        END

        INSERT INTO Orders (OrderID, [User], Products, OrderSubTotal, Discount, OrderTotal, OrderDate)
        VALUES (@OrderID, @User, @Products, @OrderSubTotal, @Discount, @OrderTotal, @OrderDate);

        UPDATE Products
        SET ProductStock = ProductStock - pq.ProductQuantity
        FROM #ProductQuantities pq
        WHERE Products.ProductID = pq.ProductID;

        COMMIT TRANSACTION;

        SELECT
            @OrderID AS OrderID,
            @User AS [User],
            @Products AS Products,
            @OrderSubTotal AS OrderSubTotal,
            @Discount AS Discount,
            @OrderTotal AS OrderTotal,
            @OrderDate AS OrderDate;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;

    DROP TABLE #ProductQuantities;
END
