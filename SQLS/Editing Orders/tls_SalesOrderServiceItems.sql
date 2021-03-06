USE [SeerysActual]
GO
/****** Object:  Table [dbo].[tls_SalesOrderServiceItems]    Script Date: 07/23/2012 09:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tls_SalesOrderServiceItems](
	[OrderServiceItemId] [int] IDENTITY(1,1) NOT NULL,
	[SalesOrderId] [int] NULL,
	[ProductId] [int] NULL,
	[ProductName] [nvarchar](200) NULL,
	[Quantity] [float] NULL,
	[Weight] [float] NULL,
	[Comment] [nvarchar](2000) NULL,
	[UnitPrice] [float] NULL,
	[VATRate] [float] NULL,
	[VAT] [float] NULL,
	[TotalPrice] [float] NULL,
	[Status] [nvarchar](50) NULL,
	[DispatchedQuantity] [float] NULL,
	[DispatchedWeight] [float] NULL
) ON [PRIMARY]
