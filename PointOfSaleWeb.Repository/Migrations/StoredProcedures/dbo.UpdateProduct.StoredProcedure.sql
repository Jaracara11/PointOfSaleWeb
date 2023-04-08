USE [Inventory]
GO
    /****** Object:  StoredProcedure [dbo].[UpdateProduct]    Script Date: 4/8/2023 10:35:27 AM ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[UpdateProduct] ---INPUT
    @ProductID INT,
    @ProductName VARCHAR(50),
    @ProductDescription VARCHAR(100),
    @ProductStock INT,
    @ProductCost DECIMAL(18, 2),
    @ProductPrice DECIMAL(18, 2),
    @ProductCategoryID INT,
    ---OUTPUT
    @UpdatedProductName VARCHAR(50) OUTPUT,
    @UpdatedProductDescription VARCHAR(100) OUTPUT,
    @UpdatedProductStock INT OUTPUT,
    @UpdatedProductCost DECIMAL(18, 2) OUTPUT,
    @UpdatedProductPrice DECIMAL(18, 2) OUTPUT,
    @UpdatedProductCategoryID INT OUTPUT AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Products
    WHERE
        ProductID = @ProductID
) BEGIN
SET
    @UpdatedProductName = NULL;

SET
    @UpdatedProductDescription = NULL;

SET
    @UpdatedProductPrice = NULL;

SET
    @UpdatedProductCost = NULL;

SET
    @UpdatedProductStock = NULL;

SET
    @UpdatedProductCategoryID = NULL;

THROW 51000,
'Product does not exist!',
1;

END IF EXISTS (
    SELECT
        1
    FROM
        Products
    WHERE
        ProductName = @ProductName
        AND ProductID <> @ProductID
) BEGIN
SET
    @UpdatedProductName = NULL;

SET
    @UpdatedProductDescription = NULL;

SET
    @UpdatedProductPrice = NULL;

SET
    @UpdatedProductCost = NULL;

SET
    @UpdatedProductStock = NULL;

SET
    @UpdatedProductCategoryID = NULL;

THROW 51000,
'Product name already exists!',
1;

END IF NOT EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryID = @ProductCategoryID
) BEGIN
SET
    @UpdatedProductName = NULL;

SET
    @UpdatedProductDescription = NULL;

SET
    @UpdatedProductPrice = NULL;

SET
    @UpdatedProductCost = NULL;

SET
    @UpdatedProductStock = NULL;

SET
    @UpdatedProductCategoryID = NULL;

THROW 51000,
'Product category does not exist!',
1;

END BEGIN TRANSACTION;

UPDATE
    Products
SET
    ProductName = @ProductName,
    ProductDescription = @ProductDescription,
    ProductPrice = @ProductPrice,
    ProductCost = @ProductCost,
    ProductStock = @ProductStock,
    ProductCategoryID = @ProductCategoryID
WHERE
    ProductID = @ProductID;

SELECT
    @UpdatedProductName = ProductName,
    @UpdatedProductDescription = ProductDescription,
    @UpdatedProductPrice = ProductPrice,
    @UpdatedProductCost = ProductCost,
    @UpdatedProductStock = ProductStock,
    @UpdatedProductCategoryID = ProductCategoryID
FROM
    Products
WHERE
    ProductID = @ProductID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @ @TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END