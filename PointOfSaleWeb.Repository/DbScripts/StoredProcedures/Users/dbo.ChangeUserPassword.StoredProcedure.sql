USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[ChangeUserPassword]    Script Date: 5/28/2023 9:49:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeUserPassword]
    @Username NVARCHAR(25),
    @OldPassword NVARCHAR(500),
	@NewPassword NVARCHAR(500)
AS
BEGIN

SET NOCOUNT ON;

DECLARE @OldPasswordVarbinary VARBINARY(500);
SET @OldPasswordVarbinary = HASHBYTES('SHA2_256', CONVERT(VARBINARY(500), @OldPassword));

 IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 51000, 'Invalid Username', 1;
    END

    IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username AND Password = @OldPasswordVarbinary)
    BEGIN
        THROW 51000, 'Invalid Password', 1;
    END

	DECLARE @NewPasswordVarbinary VARBINARY(500);
    SET @NewPasswordVarbinary = HASHBYTES('SHA2_256', @NewPassword);

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