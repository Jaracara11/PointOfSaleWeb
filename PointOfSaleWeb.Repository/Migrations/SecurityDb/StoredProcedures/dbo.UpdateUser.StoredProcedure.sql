ALTER PROCEDURE [dbo].[UpdateUser]
    @UserID INT,
    @Username NVARCHAR(50) = NULL,
    @Password NVARCHAR(500) = NULL,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @Email NVARCHAR(100) = NULL,
    @UserRoleID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID)
    BEGIN
        THROW 50001, 'User does not exist!', 1;
    END

	DECLARE @PasswordHash VARBINARY(500);
    SET @PasswordHash = HASHBYTES('SHA2_256', @Password);

    DECLARE @UpdateQuery NVARCHAR(MAX) = 'UPDATE Users SET ';

    SET @UpdateQuery += 'Username = ISNULL(@Username, Username), ';
    --SET @UpdateQuery += 'Password = ISNULL(@Password, Password), ';
	SET @UpdateQuery += 'Password = CONVERT(VARBINARY(500), ''' + CONVERT(NVARCHAR(500), @PasswordHash, 2) + '''), ';
    SET @UpdateQuery += 'FirstName = ISNULL(@FirstName, FirstName), ';
    SET @UpdateQuery += 'LastName = ISNULL(@LastName, LastName), ';
    SET @UpdateQuery += 'Email = ISNULL(@Email, Email), ';
    SET @UpdateQuery += 'UserRoleID = ISNULL(@UserRoleID, UserRoleID) ';

    SET @UpdateQuery += 'WHERE UserID = @UserID';

    BEGIN try;
        BEGIN TRANSACTION;

        EXEC sp_executesql @UpdateQuery, N'@Username NVARCHAR(50), @Password NVARCHAR(500), @FirstName NVARCHAR(50), @LastName NVARCHAR(50), @Email NVARCHAR(100), @UserRoleID INT, @UserID INT', 
            @Username, @Password, @FirstName, @LastName, @Email, @UserRoleID, @UserID;

        COMMIT TRANSACTION;

        SELECT U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
        FROM Users U WITH (NOLOCK)
        JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
        WHERE U.UserID = @UserID;

    END try

    BEGIN catch
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END catch;
END
