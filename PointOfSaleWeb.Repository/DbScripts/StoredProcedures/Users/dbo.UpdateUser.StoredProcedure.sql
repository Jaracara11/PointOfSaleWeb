USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 6/8/2023 9:43:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]
    @Username NVARCHAR(50) = NULL,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @Email NVARCHAR(100) = NULL,
    @UserRoleID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 50001, 'User does not exist!', 1;
    END

	IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleID = @UserRoleID)
    BEGIN
        THROW 50001, 'Selected Role does not exist!', 1;
    END

    DECLARE @UpdateQuery NVARCHAR(MAX) = 'UPDATE Users SET ';

    SET @UpdateQuery += 'FirstName = ISNULL(@FirstName, FirstName), ';
    SET @UpdateQuery += 'LastName = ISNULL(@LastName, LastName), ';
    SET @UpdateQuery += 'Email = ISNULL(@Email, Email), ';
    SET @UpdateQuery += 'UserRoleID = ISNULL(@UserRoleID, UserRoleID) ';

    SET @UpdateQuery += 'WHERE Username = @Username';

    BEGIN try;
        BEGIN TRANSACTION;

        EXEC sp_executesql @UpdateQuery, N'@Username NVARCHAR(50), @FirstName NVARCHAR(50), @LastName NVARCHAR(50), @Email NVARCHAR(100), @UserRoleID INT', 
            @Username, @FirstName, @LastName, @Email, @UserRoleID;

        COMMIT TRANSACTION;

        SELECT U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
        FROM Users U WITH (NOLOCK)
        JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
        WHERE U.Username = @Username;

    END try

    BEGIN catch
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END catch;
END