USE [Inventory]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 4/7/2023 1:06:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteProduct]
    @ProductID int
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductID = @ProductID)
        BEGIN
            THROW 51000, 'Product not found!', 1;
        END

        DELETE FROM Products WHERE ProductID = @ProductID;

        -- Reset identity seed of ProductID column
        DECLARE @MaxProductID INT;
        SELECT @MaxProductID = ISNULL(MAX(ProductID),0) FROM Products;
        DBCC CHECKIDENT('Products', RESEED, @MaxProductID);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END
