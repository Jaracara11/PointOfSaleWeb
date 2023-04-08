USE [Inventory]
GO
/****** Object:  StoredProcedure [dbo].[GetAllProducts]    Script Date: 4/7/2023 12:34:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProductsByCategory]
@ProductCategoryID INT
AS
BEGIN
   SELECT ProductID, ProductName, ProductDescription, ProductStock, ProductCost, ProductPrice, ProductCategoryID
   FROM Products WITH (NOLOCK) WHERE ProductCategoryID = @ProductCategoryID
   ORDER BY ProductName ASC;
END
