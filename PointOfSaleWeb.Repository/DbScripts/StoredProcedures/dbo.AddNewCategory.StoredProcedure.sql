USE [Inventory]
GO
/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 4/5/2023 8:10:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddNewCategory]
	@CategoryName varchar(50),
	@CategoryID INT = 0
AS
BEGIN
	SET NOCOUNT ON;

		IF EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = @CategoryName)
		BEGIN
			RAISERROR ('Category %s already exists!', 16, 1, @CategoryName);
			RETURN;
		END

		BEGIN

		   INSERT INTO [dbo].[Categories] (CategoryName)
		   VALUES (@CategoryName);

		   SELECT @CategoryID = SCOPE_IDENTITY();

		   EXEC GetCategoryById @CategoryID = @CategoryID;
		END

END

