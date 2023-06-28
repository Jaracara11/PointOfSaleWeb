USE [POS]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 6/24/2023 1:17:46 PM ******/
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

ALTER TABLE [dbo].[Orders] ADD  DEFAULT (newid()) FOR [OrderID]
GO