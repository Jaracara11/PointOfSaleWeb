ALTER PROCEDURE [dbo].[GetAllProducts]
AS
BEGIN
   SELECT
   ProductID,
   ProductName,
   ProductDescription,
   ProductStock,
   ProductCost,
   ProductPrice,
   ProductCategoryID
FROM
   Products WITH (NOLOCK)
ORDER BY
   ProductName ASC;
END
