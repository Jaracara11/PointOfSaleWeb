USE [Inventory]
GO
	/****** Object:  Table [dbo].[Categories]    Script Date: 3/22/2023 8:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO CREATE TABLE [dbo].[Categories](
		[CategoryID] [int] IDENTITY(1, 1) NOT NULL,
		[CategoryName] [nvarchar](50) NOT NULL,
		CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC) WITH (
			PAD_INDEX = OFF,
			STATISTICS_NORECOMPUTE = OFF,
			IGNORE_DUP_KEY = OFF,
			ALLOW_ROW_LOCKS = ON,
			ALLOW_PAGE_LOCKS = ON,
			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO