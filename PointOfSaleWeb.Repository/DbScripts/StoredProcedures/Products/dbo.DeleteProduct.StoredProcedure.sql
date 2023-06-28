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

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO