USE [Inventory]
GO
    /****** Object: StoredProcedure [dbo].[UpdateCategory] Script Date: 3/26/2023 9:10:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO CREATE PROCEDURE [dbo].[UpdateCategory] @CategoryID INT,
    @CategoryName VARCHAR(50),
    @UpdatedCategoryName VARCHAR(50) OUTPUT AS BEGIN
SET NOCOUNT ON;
BEGIN TRY IF EXISTS (
    SELECT 1
    FROM Categories
    WHERE CategoryName = @CategoryName
        AND CategoryID <> @CategoryID
) BEGIN
SET @UpdatedCategoryName = NULL;
THROW 51000,
'Category Name already exists!',
1;
END
UPDATE Categories
SET CategoryName = @CategoryName
WHERE CategoryID = @CategoryID;
SELECT @UpdatedCategoryName = CategoryName
FROM Categories
WHERE CategoryID = @CategoryID;
END TRY BEGIN CATCH THROW;
END CATCH;
END