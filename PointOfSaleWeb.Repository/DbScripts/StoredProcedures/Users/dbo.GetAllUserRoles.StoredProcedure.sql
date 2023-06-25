USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[GetAllCategories]    Script Date: 4/11/2023 10:13:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUserRoles]
AS
BEGIN
   SELECT RoleID, RoleName 
   FROM Roles WITH (NOLOCK);
END
