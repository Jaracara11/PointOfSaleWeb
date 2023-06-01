USE [Security]
GO
/****** Object:  StoredProcedure [dbo].[GetAllUserRoles]    Script Date: 4/11/2023 5:50:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUsersInfo]
AS
BEGIN
   SELECT U.UserID, U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
    FROM Users U WITH (NOLOCK)
    JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID;
END