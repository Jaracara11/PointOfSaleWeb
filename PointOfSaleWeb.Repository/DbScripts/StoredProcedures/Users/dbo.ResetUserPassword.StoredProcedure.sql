USE [POS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetUserPassword]
    @Username NVARCHAR(25),
	@NewPassword NVARCHAR(500)
AS
BEGIN

SET NOCOUNT ON;

DECLARE @NewPasswordVarbinary VARBINARY(500);
SET @NewPasswordVarbinary = HASHBYTES('SHA2_256', CONVERT(VARBINARY(500), @NewPassword));

 IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 51000, 'Invalid Username', 1;
    END

	IF EXISTS(SELECT 1 FROM Users WHERE Username = @Username AND Password = @NewPasswordVarbinary)
    BEGIN
        THROW 51000, 'New Password cannot be the same as the old password', 1;
    END

	BEGIN try;
	      BEGIN TRANSACTION;
          UPDATE Users SET Password = @NewPasswordVarbinary WHERE Username = @Username;
		  COMMIT TRANSACTION;
	 END try

	 BEGIN catch
          IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

          THROW;
      END catch;
END