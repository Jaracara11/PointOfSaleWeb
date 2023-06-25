USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 4/10/2023 9:15:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateUser]
    @Username NVARCHAR(50),
    @Password NVARCHAR(500),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 50001, 'Username already exists!', 1;
    END

    DECLARE @PasswordHash VARBINARY(500);
    SET @PasswordHash = HASHBYTES('SHA2_256', @Password);

	BEGIN try;
	      BEGIN TRANSACTION;
          INSERT INTO Users (Username, Password, FirstName, LastName, Email)
          VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Email);
		  COMMIT TRANSACTION;

		  DECLARE @UserID INT;

		  SELECT @UserID = SCOPE_IDENTITY();

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
