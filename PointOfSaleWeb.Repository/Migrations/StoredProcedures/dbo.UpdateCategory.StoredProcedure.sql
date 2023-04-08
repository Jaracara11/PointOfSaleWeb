USE [Inventory]
GO
    /****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 4/8/2023 8:22:17 AM ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    ALTER PROCEDURE [dbo].[UpdateCategory] @CategoryID INT,
    @CategoryName VARCHAR(50),
    @UpdatedCategoryName VARCHAR(50) OUTPUT AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryID = @CategoryID
) BEGIN
SET
    @UpdatedCategoryName = NULL;

THROW 51000,
'Category does not exist!',
1;

END IF EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryName = @CategoryName
) BEGIN
SET
    @UpdatedCategoryName = NULL;

THROW 51000,
'Category name already exists!',
1;

END
UPDATE
    Categories
SET
    CategoryName = @CategoryName
WHERE
    CategoryID = @CategoryID;

SELECT
    @UpdatedCategoryName = CategoryName
FROM
    Categories
WHERE
    CategoryID = @CategoryID;

END TRY BEGIN CATCH THROW;

END CATCH;

END