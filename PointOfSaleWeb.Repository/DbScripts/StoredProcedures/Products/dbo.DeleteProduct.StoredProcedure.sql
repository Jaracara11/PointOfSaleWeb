USE [POS]
GO
    /****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 4/8/2023 5:21:54 PM ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[DeleteProduct] @ProductID int AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Products
    WHERE
        ProductID = @ProductID
) BEGIN THROW 51000,
'Product not found!',
1;

END BEGIN TRANSACTION;

DELETE FROM
    Products
WHERE
    ProductID = @ProductID;

COMMIT TRANSACTION;

-- Reset identity seed of ProductID column
DECLARE @MaxProductID INT;

SELECT
    @MaxProductID = ISNULL(MAX(ProductID), 0)
FROM
    Products;

DBCC CHECKIDENT('Products', RESEED, @MaxProductID);

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO