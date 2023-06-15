USE [POS]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Discounts](
	[UserRoleID] [int] NOT NULL,
	[DiscountsAvailable] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO

-- Insert discounts
INSERT INTO [dbo].[Discounts] ([UserRoleID], [DiscountsAvailable])
VALUES (1, '0.2, 0.15, 0.1, 0.05'),
       (2, '0.2, 0.15, 0.1, 0.05'),
       (3, '0.1, 0.05');
GO
