USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[GetAllProducts]    Script Date: 5/25/2023 4:59:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllProducts]
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
