USE Inventory;

GO

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [dbo].[AddNewProduct]
    @ProductID INT = 0,
    @ProductName nvarchar(50),
    @ProductDescription nvarchar(100) = NULL,
    @ProductPrice decimal(10,2),
    @ProductCost decimal(10,2),
    @ProductStock int,
    @ProductCategoryID int
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [ProductName] = @ProductName)
    BEGIN
        RAISERROR ('Product %s already exists!', 16, 1, @ProductName);
        RETURN;
    END

    BEGIN

        INSERT INTO [dbo].[Products] (ProductName, ProductDescription, ProductPrice, ProductCost, ProductStock, ProductCategoryID)
        VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductCost, @ProductStock, @ProductCategoryID);

        SELECT @ProductID = SCOPE_IDENTITY();

        EXEC GetProductById @ProductID = @ProductID;
    END
END
