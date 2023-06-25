USE [POS]
GO
    /****** Object: StoredProcedure [dbo].[UpdateProduct] ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[UpdateProduct] @ProductID INT,
    @ProductName VARCHAR(50),
    @ProductDescription VARCHAR(100),
    @ProductStock INT,
    @ProductCost DECIMAL(18, 2),
    @ProductPrice DECIMAL(18, 2),
    @ProductCategoryID INT AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        products
    WHERE
        productid = @ProductID
) BEGIN THROW 51000,
'Product does not exist!',
1;

END;

IF EXISTS (
    SELECT
        1
    FROM
        products
    WHERE
        productname = @ProductName
        AND ProductID <> @ProductID
) BEGIN THROW 51000,
'Product name already exists!',
1;

END;

IF NOT EXISTS (
    SELECT
        1
    FROM
        categories
    WHERE
        CategoryID = @ProductCategoryID
) BEGIN THROW 51000,
'Product category does not exist!',
1;

END;

BEGIN TRANSACTION;

UPDATE
    products
SET
    productname = @ProductName,
    productdescription = @ProductDescription,
    productprice = @ProductPrice,
    productcost = @ProductCost,
    productstock = @ProductStock,
    productcategoryid = @ProductCategoryID
WHERE
    productid = @ProductID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END