USE [Inventory]
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryById]    Script Date: 3/22/2023 8:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCategoryById]
    @CategoryId INT
AS
BEGIN
    SELECT CategoryID, CategoryName 
    FROM Categories WITH (NOLOCK)
    WHERE CategoryID = @CategoryId
END
GO
