USE [POS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthUser]
    @Username NVARCHAR(25),
    @Password NVARCHAR(500)

AS
BEGIN

SET NOCOUNT ON;

DECLARE @PasswordVarbinary VARBINARY(500);
SET @PasswordVarbinary = HASHBYTES('SHA2_256', CONVERT(VARBINARY(500), @Password));

 IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 51000, 'Invalid Username', 1;
    END

    IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username AND Password = @PasswordVarbinary)
    BEGIN
        THROW 51000, 'Invalid Password', 1;
    END

	SELECT U.UserID, U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
    FROM Users U WITH (NOLOCK)
    JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
    WHERE U.Username = @Username AND U.Password = @PasswordVarbinary;
END