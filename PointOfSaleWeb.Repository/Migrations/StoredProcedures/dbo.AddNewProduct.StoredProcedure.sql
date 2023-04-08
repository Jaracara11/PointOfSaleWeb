USE [Inventory]
GO

/****** Object:  StoredProcedure [dbo].[AddNewProduct]    Script Date: 4/8/2023 11:30:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[AddNewProduct]
    @ProductID INT = 0,
    @ProductName NVARCHAR(50),
    @ProductDescription NVARCHAR(100) = NULL,
    @ProductPrice DECIMAL(10, 2),
    @ProductCost DECIMAL(10, 2),
    @ProductStock INT,
    @ProductCategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

	  IF EXISTS (SELECT 1
           FROM [dbo].[products]
           WHERE [productname] = @ProductName)
BEGIN
    THROW 50000, 'Product name already exists!', 1;
END

  
    IF NOT EXISTS (SELECT 1
                   FROM categories
                   WHERE categoryid = @ProductCategoryID)
    BEGIN
        THROW 51000, 'Product category does not exist!', 1;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [dbo].[products]
               (productname,
                productdescription,
                productprice,
                productcost,
                productstock,
                productcategoryid)
        VALUES (@ProductName,
                @ProductDescription,
                @ProductPrice,
                @ProductCost,
                @ProductStock,
                @ProductCategoryID);

        COMMIT TRANSACTION;

        SELECT @ProductID = SCOPE_IDENTITY();

        EXEC GetProductByID @ProductID = @ProductID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(MAX);
        SET @ErrorMessage = 'Error occurred in ' + ERROR_PROCEDURE() + ': ' + ERROR_MESSAGE();

        THROW 51001, @ErrorMessage, 1;
    END CATCH;

    RETURN 0;
END
