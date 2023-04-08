USE [Inventory]
GO
    /****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 4/8/2023 11:46:03 AM ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    ALTER PROCEDURE [dbo].[UpdateCategory] @CategoryID INT,
    @CategoryName VARCHAR(50) AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryID = @CategoryID
) BEGIN THROW 51000,
'Category does not exist!',
1;

END IF EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryName = @CategoryName
) BEGIN THROW 51000,
'Category name already exists!',
1;

END BEGIN TRANSACTION;

UPDATE
    Categories
SET
    CategoryName = @CategoryName
WHERE
    CategoryID = @CategoryID;

COMMIT TRANSACTION;

EXEC Getcategorybyid @CategoryID = @CategoryID;

END TRY BEGIN CATCH IF @ @TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END