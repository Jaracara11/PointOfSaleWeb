USE [POS]
GO
	/****** Object:  Table [dbo].[Products]    Script Date: 4/8/2023 5:21:54 PM ******/
SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	CREATE TABLE [dbo].[Products](
		[ProductID] [int] IDENTITY(1, 1) NOT NULL,
		[ProductName] [nvarchar](50) NOT NULL,
		[ProductDescription] [nvarchar](100) NULL,
		[ProductPrice] [decimal](10, 2) NOT NULL,
		[ProductCost] [decimal](10, 2) NOT NULL,
		[ProductStock] [int] NOT NULL,
		[ProductCategoryID] [int] NOT NULL,
		CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductID] ASC) WITH (
			PAD_INDEX = OFF,
			STATISTICS_NORECOMPUTE = OFF,
			IGNORE_DUP_KEY = OFF,
			ALLOW_ROW_LOCKS = ON,
			ALLOW_PAGE_LOCKS = ON,
			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO
ALTER TABLE
	[dbo].[Products] WITH CHECK
ADD
	CONSTRAINT [fk_products_categoryID] FOREIGN KEY([ProductCategoryID]) REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE
	[dbo].[Products] CHECK CONSTRAINT [fk_products_categoryID]
GO