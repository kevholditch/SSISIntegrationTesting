/****** Object:  Table [dbo].[Products]    Script Date: 06/04/2017 14:32:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[ProductCode] [int] NOT NULL,
	[ShippingWeight] [float] NOT NULL,
	[ShippingLength] [float] NOT NULL,
	[ShippingWidth] [float] NOT NULL,
	[ShippingHeight] [float] NOT NULL,
	[UnitCost] [float] NOT NULL,
	[PerOrder] [tinyint] NOT NULL
) ON [PRIMARY]

GO