/****** Object:  StoredProcedure [dbo].[prd_insReportLine]    Script Date: 03/14/2011 10:51:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create PROCEDURE [dbo].[prd_insReportLine]	
@DocketNum nvarchar(50),
@DateReceived datetime,
@PurOrderNum nvarchar(50),
@OrderItemID int,
@SupplierDesc nvarchar(50),
@QtyReceived float,
@TotalPrice float
 
AS

insert into trc_Report
(
ProductCode,
StartDate,
Van,
Age, 
ProductName,
QtyOutToVan,
QtySold
)
values
(
@DocketNum,
@DateReceived,
@PurOrderNum,
@OrderItemId,
@SupplierDesc,
@QtyReceived,
@TotalPrice 
)



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



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resDataForSerialNumAndTraceID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resDataForSerialNumAndTraceID]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE trc_resDataForSerialNumAndTraceID	@SerialNum nvarchar(50),
								@TraceCodeId int
AS

select distinct(SerialNum), ProductId, TraceCodeId, Barcode, LocationId, SellByDate
from trc_Transactions where serialNum = @SerialNum and TraceCodeId = @TraceCodeId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resDataForSerialNumTrace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resDataForSerialNumTrace]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE trc_resDataForSerialNumTrace	@SerialNum nvarchar(50),
						@TraceCodeId int = 0
AS


if @TraceCodeId = 0 
	
	select distinct(SerialNum), ProductId, TraceCodeId, Barcode, LocationId, SellByDate
	from trc_Transactions where serialNum = @SerialNum

else

	select distinct(SerialNum), ProductId, TraceCodeId, Barcode, LocationId, SellByDate
	from trc_Transactions where serialNum = @SerialNum and TraceCodeId = @TraceCodeId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tcd_updCustomer]and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tcd_updCustomer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tcd_updCustomer]
	@CustomerId int,
	@CustomerName nchar(255),
	@Address ntext,
	@DiscountPercent float,
	@IsVatExempt bit,
	@Route nchar(50),
	@PaymentType nchar(50) = ' '
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

update tcd_tblCustomers set CustomerName = @CustomerName, Address = @Address, DiscountPercent = @DiscountPercent,
IsVatExempt = @IsVatExempt, Route = @Route, PaymentType = @PaymentType where CustomerID = @CustomerId

END


