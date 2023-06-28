CREATE TRIGGER ResetProductIDSeed
ON Products
AFTER DELETE
AS
BEGIN
    DECLARE @MaxProductID INT;

    SELECT @MaxProductID = ISNULL(MAX(ProductID), 0)
    FROM Products;

    DBCC CHECKIDENT('Products', RESEED, @MaxProductID);
END;
