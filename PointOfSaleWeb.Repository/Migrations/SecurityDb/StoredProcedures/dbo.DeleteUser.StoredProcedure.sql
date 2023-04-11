USE [Security]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 4/10/2023 8:40:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUser]
    @UserID int
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID)
        BEGIN
            THROW 51000, 'User not found!', 1;
        END

        BEGIN TRANSACTION;
        DELETE FROM Users WHERE UserID = @UserID;
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
