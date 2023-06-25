USE [POS]
GO
    /****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 5/24/2023 4:53:28 PM ******/
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[UpdateCategory] @CategoryID INT,
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

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END