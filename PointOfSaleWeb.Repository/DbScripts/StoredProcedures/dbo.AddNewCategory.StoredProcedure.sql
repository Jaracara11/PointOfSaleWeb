USE [Inventory]
GO
/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 4/1/2023 8:45:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddNewCategory]
    @CategoryName varchar(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Categories WHERE CategoryName = @CategoryName)
        BEGIN
            THROW 51000, 'Category Name already exists!', 1;
        END

        INSERT INTO Categories (CategoryName)
        VALUES (@CategoryName);

        SELECT c.CategoryID, c.CategoryName
        FROM Categories c
        WHERE c.CategoryID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END
