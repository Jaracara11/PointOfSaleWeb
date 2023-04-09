USE [Security]
GO
/****** Object:  StoredProcedure [dbo].[GetUserLoginData]    Script Date: 4/9/2023 11:44:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserLoginData]
    @Username NVARCHAR(25),
    @Password NVARCHAR(25)
AS
BEGIN
 IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 51000, 'Invalid Username', 1;
    END

    IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username AND Password = @Password)
    BEGIN
        THROW 51000, 'Invalid Password', 1;
    END

	SELECT U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
    FROM Users U WITH (NOLOCK)
    JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
    WHERE U.Username = @Username AND U.Password = @Password;
END
GO
