USE [master]
GO
/****** Object:  Database [POS]    Script Date: 6/28/2023 2:43:48 PM ******/
CREATE DATABASE [POS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'POS', FILENAME = N'/var/opt/mssql/data/POS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'POS_log', FILENAME = N'/var/opt/mssql/data/POS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [POS] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [POS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [POS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [POS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [POS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [POS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [POS] SET ARITHABORT OFF 
GO
ALTER DATABASE [POS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [POS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [POS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [POS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [POS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [POS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [POS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [POS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [POS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [POS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [POS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [POS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [POS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [POS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [POS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [POS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [POS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [POS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [POS] SET  MULTI_USER 
GO
ALTER DATABASE [POS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [POS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [POS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [POS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [POS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [POS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'POS', N'ON'
GO
ALTER DATABASE [POS] SET QUERY_STORE = OFF
GO
USE [POS]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discounts]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discounts](
	[UserRoleID] [int] NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [uniqueidentifier] NOT NULL,
	[User] [nvarchar](25) NOT NULL,
	[Products] [nvarchar](max) NOT NULL,
	[OrderSubTotal] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NULL,
	[OrderTotal] [decimal](18, 2) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[ProductDescription] [nvarchar](100) NULL,
	[ProductPrice] [decimal](10, 2) NOT NULL,
	[ProductCost] [decimal](10, 2) NOT NULL,
	[ProductStock] [int] NOT NULL,
	[ProductCategoryID] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] NOT NULL,
	[RoleName] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](25) NOT NULL,
	[Password] [varbinary](500) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[UserRoleID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (newid()) FOR [OrderID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [UserRoleID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [fk_products_categoryID] FOREIGN KEY([ProductCategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [fk_products_categoryID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [fk_Users_RoleID] FOREIGN KEY([UserRoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [fk_Users_RoleID]
GO
/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE PROCEDURE [dbo].[AddNewCategory] @CategoryName VARCHAR(50),
  @CategoryID INT = 0 AS BEGIN
SET
  nocount ON;

IF EXISTS (
  SELECT
    1
  FROM
    [dbo].[Categories]
  WHERE
    [Categoryname] = @CategoryName
) BEGIN THROW 50000,
'Category name already exists!',
1;

END BEGIN try;

BEGIN TRANSACTION;

INSERT INTO
  [dbo].[Categories] (Categoryname)
VALUES
  (@CategoryName);

COMMIT TRANSACTION;

SELECT
  @CategoryID = Scope_identity();

EXEC Getcategorybyid @CategoryID = @CategoryID;

END try BEGIN catch IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END catch;

END
GO
/****** Object:  StoredProcedure [dbo].[AddNewProduct]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[AddNewProduct] @ProductID INT = 0,
    @ProductName NVARCHAR(50),
    @ProductDescription NVARCHAR(100) = NULL,
    @ProductPrice DECIMAL(10, 2),
    @ProductCost DECIMAL(10, 2),
    @ProductStock INT,
    @ProductCategoryID INT AS BEGIN
SET
    NOCOUNT ON;

IF EXISTS (
    SELECT
        1
    FROM
        [dbo].[products]
    WHERE
        [productname] = @ProductName
) BEGIN THROW 50000,
'Product name already exists!',
1;

END IF NOT EXISTS (
    SELECT
        1
    FROM
        categories
    WHERE
        categoryid = @ProductCategoryID
) BEGIN THROW 51000,
'Product category does not exist!',
1;

END BEGIN TRY BEGIN TRANSACTION;

INSERT INTO
    [dbo].[products] (
        productname,
        productdescription,
        productprice,
        productcost,
        productstock,
        productcategoryid
    )
VALUES
    (
        @ProductName,
        @ProductDescription,
        @ProductPrice,
        @ProductCost,
        @ProductStock,
        @ProductCategoryID
    );

COMMIT TRANSACTION;

SELECT
    @ProductID = SCOPE_IDENTITY();

EXEC GetProductByID @ProductID = @ProductID;

END TRY BEGIN catch IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END catch;

END
GO
/****** Object:  StoredProcedure [dbo].[AuthUser]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthUser]
    @Username NVARCHAR(25),
    @Password NVARCHAR(500)

AS
BEGIN

SET NOCOUNT ON;

DECLARE @PasswordVarbinary VARBINARY(500);
SET @PasswordVarbinary = HASHBYTES('SHA2_256', CONVERT(VARBINARY(500), @Password));

 IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 51000, 'Invalid Username', 1;
    END

    IF NOT EXISTS(SELECT 1 FROM Users WHERE Username = @Username AND Password = @PasswordVarbinary)
    BEGIN
        THROW 51000, 'Invalid Password', 1;
    END

	SELECT U.UserID, U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
    FROM Users U WITH (NOLOCK)
    JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
    WHERE U.Username = @Username AND U.Password = @PasswordVarbinary;
END
GO
/****** Object:  StoredProcedure [dbo].[ChangeUserPassword]    Script Date: 6/28/2023 2:43:48 PM ******/
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
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 6/28/2023 2:43:48 PM ******/
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
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[DeleteCategory] @CategoryID INT AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY -- Check if the category exists
IF NOT EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryID = @CategoryID
) BEGIN THROW 51000,
'Category not found!',
1;

END -- Check if any products exist with the specified CategoryID
IF EXISTS (
    SELECT
        1
    FROM
        Products
    WHERE
        ProductCategoryID = @CategoryID
) BEGIN THROW 51000,
'Cannot delete this category while it has products assigned to it!',
1;

END BEGIN TRANSACTION;

-- Delete the category
DELETE FROM
    Categories
WHERE
    CategoryID = @CategoryID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[DeleteProduct] @ProductID int AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Products
    WHERE
        ProductID = @ProductID
) BEGIN THROW 51000,
'Product not found!',
1;

END BEGIN TRANSACTION;

DELETE FROM
    Products
WHERE
    ProductID = @ProductID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUser]
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

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllCategories]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[GetAllCategories] AS BEGIN
SELECT
   CategoryID,
   CategoryName
FROM
   Categories WITH (NOLOCK)
ORDER BY
   CategoryName ASC;

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllDiscounts]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[GetAllDiscounts] AS BEGIN
SELECT
   UserRoleID,
   DiscountAmount
FROM
   Discounts WITH (NOLOCK)
   ORDER BY DiscountAmount ASC
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllProducts]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllProducts]
AS
BEGIN
   SELECT
   ProductID,
   ProductName,
   ProductDescription,
   ProductStock,
   ProductCost,
   ProductPrice,
   ProductCategoryID
FROM
   Products WITH (NOLOCK)
ORDER BY
   ProductName ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllUserRoles]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUserRoles]
AS
BEGIN
   SELECT RoleID, RoleName 
   FROM Roles WITH (NOLOCK);
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllUsers]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUsers]
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
END
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryById]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[GetCategoryById] @CategoryId INT AS BEGIN
SELECT
    CategoryID,
    CategoryName
FROM
    Categories WITH (NOLOCK)
WHERE
    CategoryID = @CategoryId
END
GO
/****** Object:  StoredProcedure [dbo].[GetDiscountsByUsername]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[GetDiscountsByUsername] 
   @Username NVARCHAR(50)

AS 

BEGIN

DECLARE @UserRoleID INT = (SELECT UserRoleID FROM Users WHERE Username = @Username)

SELECT
   DiscountAmount
FROM
   Discounts WITH (NOLOCK)
   WHERE UserRoleID = @UserRoleID
   ORDER BY DiscountAmount ASC
END
GO
/****** Object:  StoredProcedure [dbo].[GetProductById]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[GetProductById] @ProductId INT AS BEGIN
SELECT
    ProductID,
    ProductName,
    ProductDescription,
    ProductStock,
    ProductCost,
    ProductPrice,
    ProductCategoryID
FROM
    Products WITH (NOLOCK)
WHERE
    ProductID = @ProductId
END
GO
/****** Object:  StoredProcedure [dbo].[GetProductsByCategoryId]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[GetProductsByCategoryId] @ProductCategoryID INT AS BEGIN
SELECT
   ProductID,
   ProductName,
   ProductDescription,
   ProductStock,
   ProductCost,
   ProductPrice,
   ProductCategoryID
FROM
   Products WITH (NOLOCK)
WHERE
   ProductCategoryID = @ProductCategoryID
ORDER BY
   ProductName ASC;

END
GO
/****** Object:  StoredProcedure [dbo].[GetUserByUsername]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[NewOrderTransaction]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NewOrderTransaction]
    @User NVARCHAR(25),
    @Products NVARCHAR(MAX),
    @Discount DECIMAL(18, 2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1
                   FROM   Users
                   WHERE  Username = @User)
    BEGIN
        THROW 51000, 'User does not exist!', 1;
    END 

	DECLARE @UserRoleID INT;

	SELECT @UserRoleID = (SELECT UserRoleID FROM Users WHERE Username = @User);

	IF (@UserRoleID = 0 OR @UserRoleID IS NULL)
	BEGIN
        THROW 51000, 'User does not have permission to perform sales operations.', 1;
    END 

    IF @Discount IS NOT NULL
    BEGIN
	   DECLARE @DiscountValid DECIMAL(18, 2);
       SELECT @DiscountValid = DiscountAmount
       FROM Discounts WITH (NOLOCK)
       WHERE UserRoleID = @UserRoleID AND DiscountAmount = @Discount;
    END

    IF @DiscountValid IS NULL AND @Discount IS NOT NULL
    BEGIN
        THROW 50002, 'Invalid discount amount, please try again.', 1;
    END

    CREATE TABLE #ProductQuantities
    (
        ProductID INT,
        ProductQuantity INT
    );

    INSERT INTO #ProductQuantities (ProductID, ProductQuantity)
    SELECT JSON_VALUE(p.Value, '$.ProductID') AS ProductID,
           JSON_VALUE(p.Value, '$.ProductQuantity') AS ProductQuantity
    FROM OPENJSON(@Products) AS p;

    IF EXISTS (
        SELECT 1
        FROM #ProductQuantities pq
        INNER JOIN Products p ON pq.ProductID = p.ProductID
        WHERE pq.ProductQuantity > p.ProductStock
    )
    BEGIN
        DROP TABLE #ProductQuantities;
        THROW 50001, 'Product quantity exceeds its stock, please update your cart.', 1;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OrderID NVARCHAR(50) = NEWID();
        DECLARE @OrderSubTotal DECIMAL(18, 2) = 0;
        DECLARE @OrderTotal DECIMAL(18, 2) = 0;
        DECLARE @OrderDate DATETIME = GETDATE();
        
        SELECT @OrderSubTotal = SUM(pq.ProductQuantity * p.ProductPrice)
        FROM #ProductQuantities pq
        INNER JOIN Products p ON pq.ProductID = p.ProductID;

        IF @Discount IS NOT NULL
        BEGIN
            SET @OrderTotal = @OrderSubTotal - (@OrderSubTotal * @Discount);
        END
        ELSE
        BEGIN
            SET @OrderTotal = @OrderSubTotal;
        END

        INSERT INTO Orders (OrderID, [User], Products, OrderSubTotal, Discount, OrderTotal, OrderDate)
        VALUES (@OrderID, @User, @Products, @OrderSubTotal, @Discount, @OrderTotal, @OrderDate);

        UPDATE Products
        SET ProductStock = ProductStock - pq.ProductQuantity
        FROM #ProductQuantities pq
        WHERE Products.ProductID = pq.ProductID;

        COMMIT TRANSACTION;

        SELECT
            @OrderID AS OrderID,
            @User AS [User],
            @Products AS Products,
            @OrderSubTotal AS OrderSubTotal,
            @Discount AS Discount,
            @OrderTotal AS OrderTotal,
            @OrderDate AS OrderDate;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;

    DROP TABLE #ProductQuantities;
END
GO
/****** Object:  StoredProcedure [dbo].[ResetUserPassword]    Script Date: 6/28/2023 2:43:48 PM ******/
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
GO
/****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[UpdateCategory] @CategoryID INT,
    @CategoryName VARCHAR(50) AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryID = @CategoryID
) BEGIN THROW 51000,
'Category does not exist!',
1;

END IF EXISTS (
    SELECT
        1
    FROM
        Categories
    WHERE
        CategoryName = @CategoryName
) BEGIN THROW 51000,
'Category name already exists!',
1;

END BEGIN TRANSACTION;

UPDATE
    Categories
SET
    CategoryName = @CategoryName
WHERE
    CategoryID = @CategoryID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProduct]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE PROCEDURE [dbo].[UpdateProduct] @ProductID INT,
    @ProductName VARCHAR(50),
    @ProductDescription VARCHAR(100),
    @ProductStock INT,
    @ProductCost DECIMAL(18, 2),
    @ProductPrice DECIMAL(18, 2),
    @ProductCategoryID INT AS BEGIN
SET
    NOCOUNT ON;

BEGIN TRY IF NOT EXISTS (
    SELECT
        1
    FROM
        products
    WHERE
        productid = @ProductID
) BEGIN THROW 51000,
'Product does not exist!',
1;

END;

IF EXISTS (
    SELECT
        1
    FROM
        products
    WHERE
        productname = @ProductName
        AND ProductID <> @ProductID
) BEGIN THROW 51000,
'Product name already exists!',
1;

END;

IF NOT EXISTS (
    SELECT
        1
    FROM
        categories
    WHERE
        CategoryID = @ProductCategoryID
) BEGIN THROW 51000,
'Product category does not exist!',
1;

END;

BEGIN TRANSACTION;

UPDATE
    products
SET
    productname = @ProductName,
    productdescription = @ProductDescription,
    productprice = @ProductPrice,
    productcost = @ProductCost,
    productstock = @ProductStock,
    productcategoryid = @ProductCategoryID
WHERE
    productid = @ProductID;

COMMIT TRANSACTION;

END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

THROW;

END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 6/28/2023 2:43:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]
    @Username NVARCHAR(50) = NULL,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @Email NVARCHAR(100) = NULL,
    @UserRoleID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        THROW 50001, 'User does not exist!', 1;
    END

	IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleID = @UserRoleID)
    BEGIN
        THROW 50001, 'Selected Role does not exist!', 1;
    END

    DECLARE @UpdateQuery NVARCHAR(MAX) = 'UPDATE Users SET ';

    SET @UpdateQuery += 'FirstName = ISNULL(@FirstName, FirstName), ';
    SET @UpdateQuery += 'LastName = ISNULL(@LastName, LastName), ';
    SET @UpdateQuery += 'Email = ISNULL(@Email, Email), ';
    SET @UpdateQuery += 'UserRoleID = ISNULL(@UserRoleID, UserRoleID) ';

    SET @UpdateQuery += 'WHERE Username = @Username';

    BEGIN try;
        BEGIN TRANSACTION;

        EXEC sp_executesql @UpdateQuery, N'@Username NVARCHAR(50), @FirstName NVARCHAR(50), @LastName NVARCHAR(50), @Email NVARCHAR(100), @UserRoleID INT', 
            @Username, @FirstName, @LastName, @Email, @UserRoleID;

        COMMIT TRANSACTION;

        SELECT U.Username, CONCAT(U.FirstName, ' ', U.LastName) AS Name, U.Email, R.RoleName AS Role
        FROM Users U WITH (NOLOCK)
        JOIN Roles R WITH (NOLOCK) ON U.UserRoleID = R.RoleID
        WHERE U.Username = @Username;

    END try

    BEGIN catch
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END catch;
END
GO
USE [master]
GO
ALTER DATABASE [POS] SET  READ_WRITE 
GO