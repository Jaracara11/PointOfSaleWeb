USE [POS]
GO
SET
    ANSI_NULLS ON
GO
SET
    QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[GetUserById] @UserId INT 
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
    UserID = @UserId
END
GO