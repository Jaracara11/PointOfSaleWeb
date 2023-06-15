USE [Proyecto11]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 5/28/2023 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteUser]
    @Username NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
        BEGIN
            THROW 51000, 'User not found!', 1;
        END

        BEGIN TRANSACTION;
        DELETE FROM Users WHERE Username = @Username;
        COMMIT TRANSACTION;

        -- Reset identity seed of CategoryID column
        DECLARE @MaxUserID INT;
        SELECT @MaxUserID = ISNULL(MAX(UserID),0) FROM Users;
        DBCC CHECKIDENT('Users', RESEED, @MaxUserID);

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
