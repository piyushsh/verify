if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tcd_insFulfillSalesOrderItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tcd_insFulfillSalesOrderItem]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


Create PROCEDURE [dbo].[tcd_insFulfillSalesOrderItem]	

	@OrderNumber int,
	@OrderDate datetime,
	@CustomerId int,
	@ProductCode nvarchar(50),
	@QtyOrWeight float,
	@Quantity int,
	@OrderComplete int,
	@DriverId int,
	@TransactionType int,
	@CommodityCode nvarchar(50),
	@PONumber nvarchar(50),
	@ItemId int,
	@SaleType int,
	@TraceCode nvarchar(50),
	@VAT float,
	@Price float,@DeliveryCustomerId int, @Origin nvarchar(50), @LocationId int, @ChargedByType nvarchar(20), 
	@SerialNum nvarchar(50), @Barcode nvarchar(50)
AS

insert into tcd_tblSalesOrders
(
	OrderNumber,
	OrderDate,
	CustomerId,
	ProductId,
	QtyOrWeight,
	Quantity,
	OrderComplete,
	DriverId,
	TransactionType,
	CommodityCode,
	PONumber,
	ItemNumber,
	SaleType,
	DocketNumber,
	TraceCode,
	DateOfTransaction,
	PriceSoldFor,
	Location,
	VatCharged,
	DeliveryCustomerID,
	IsDownloaded, Origin, ChargedByType, OrderedQty, OrderedWgt,
	SerialNum, Barcode
)
values
(
	@OrderNumber,
	@OrderDate,
	@CustomerId,
	@ProductCode,
	@QtyOrWeight,
	@Quantity,
	@OrderComplete,
	@DriverId,
	@TransactionType,
	@CommodityCode,
	@PONumber,
	@ItemId,
	@SaleType,
	0,
	@TraceCode,
	@OrderDate,
	@Price,
	@LocationId,
	@VAT,
	@DeliveryCustomerId,
	1, @Origin, @ChargedByType,0,0,
	@SerialNum, @Barcode

)
