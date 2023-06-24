USE [POS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[NewOrderTransaction]
    @User NVARCHAR(25),
    @Products NVARCHAR(MAX),
    @Discount DECIMAL(18, 2),
    @OrderTotal DECIMAL(18, 2),
    @OrderDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

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

        DECLARE @OrderID NVARCHAR(50);
        SET @OrderID = NEWID();

        INSERT INTO Orders (OrderID, Products, [User], Discount, OrderTotal, OrderDate)
        VALUES (@OrderID, @Products, @User, @Discount, @OrderTotal, @OrderDate);

        UPDATE Products
        SET ProductStock = ProductStock - pq.ProductQuantity
        FROM #ProductQuantities pq
        WHERE Products.ProductID = pq.ProductID;

        COMMIT TRANSACTION;

        SELECT
            @OrderID AS OrderID,
            @User AS [User],
            (@OrderTotal - (@Discount * 100)) AS OrderSubTotal,
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
