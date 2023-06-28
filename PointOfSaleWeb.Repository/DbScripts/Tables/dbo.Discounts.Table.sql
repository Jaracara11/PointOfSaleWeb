USE [POS]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Discounts](
	[UserRoleID] [int] NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL
) ON [PRIMARY]
GO

-- Insert discounts
INSERT INTO [dbo].[Discounts] ([UserRoleID], [DiscountAmount])
VALUES (1, 0.20),
       (1, 0.15),
	   (1, 0.10),
	   (1, 0.05),
	   (2, 0.20),
       (2, 0.15),
	   (2, 0.10),
	   (2, 0.05),
	   (3, 0.10),
	   (3, 0.05)
GO