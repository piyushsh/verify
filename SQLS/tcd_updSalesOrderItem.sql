if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tcd_updSalesOrderItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tcd_updSalesOrderItem]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO






CREATE PROCEDURE tcd_updSalesOrderItem	

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
	@Price float,@DeliveryCustomerId int, @Origin nvarchar(50), @LocationId int, @ChargedByType nvarchar(20)
AS

begin update tcd_tblSalesOrders

set OrderNumber =@OrderNumber,
OrderDate =@OrderDate,
CustomerId =@CustomerId,
ProductId =@ProductCode,
QtyOrWeight =0,
Quantity=0,
OrderComplete=@OrderComplete,
DriverId=@DriverId,
TransactionType=@TransactionType,
CommodityCode=@CommodityCode,
PONumber=@PONumber,
SaleType=@SaleType,
DocketNumber=0,
TraceCode=@TraceCode,
DateOfTransaction=@OrderDate,
PriceSoldFor=@Price,
Location=@LocationId,
VatCharged=@VAT,
DeliveryCustomerID=@DeliveryCustomerId,
IsDownloaded=1, 
Origin=@Origin, 
ChargedByType=@ChargedByType, 
OrderedQty=@Quantity, 
OrderedWgt=@QtyOrWeight
where ItemNumber = @ItemId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

