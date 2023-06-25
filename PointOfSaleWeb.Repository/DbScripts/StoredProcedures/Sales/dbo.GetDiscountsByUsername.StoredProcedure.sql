USE [POS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[GetDiscountsByUsername] 
   @Username NVARCHAR(50)

AS 

BEGIN

DECLARE @UserRoleID INT = (SELECT UserRoleID FROM Users WHERE Username = @Username)

SELECT
   DiscountAmount
FROM
   Discounts WITH (NOLOCK)
   WHERE UserRoleID = @UserRoleID
   ORDER BY DiscountAmount ASC
END
