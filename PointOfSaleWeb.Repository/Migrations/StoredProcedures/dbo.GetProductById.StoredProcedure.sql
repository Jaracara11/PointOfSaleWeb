USE [Inventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO CREATE PROCEDURE [dbo].[GetProductById] @ProductId INT AS BEGIN
SELECT ProductID,
    ProductName,
    ProductDescription,
    ProductStock,
    ProductCost,
    ProductPrice,
    ProductCategoryID
FROM Products WITH (NOLOCK)
WHERE ProductID = @ProductId
END