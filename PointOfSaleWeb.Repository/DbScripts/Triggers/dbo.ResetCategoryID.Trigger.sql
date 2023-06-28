CREATE TRIGGER ResetCategoryIDSeed
ON Categories
AFTER DELETE
AS
BEGIN
    DECLARE @MaxCategoryID INT;

    SELECT @MaxCategoryID = ISNULL(MAX(CategoryID), 0)
    FROM Categories;

    DBCC CHECKIDENT('Categories', RESEED, @MaxCategoryID);
END;
