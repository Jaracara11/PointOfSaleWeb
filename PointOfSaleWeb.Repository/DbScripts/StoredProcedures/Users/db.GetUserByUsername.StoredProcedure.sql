USE [POS]
GO
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[GetUserByUsername] @Username NVARCHAR(50)
    AS 
    BEGIN
SELECT
    Username,
    FirstName,
    LastName,
    Email,
    UserRoleID
FROM
    Users WITH (NOLOCK)
WHERE
    Username = @Username
END
GO