if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_insSalesCollectionTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_insSalesCollectionTransaction]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE trc_insSalesCollectionTransaction	 @ProductId int,
						@TraceCodeId int,
						@Driver nchar(50),
						@DateOfTransaction datetime,
						@TransactionType int,
					    	@Quantity int,
						@Weight float,
						@PONumber nchar(50),
						@CustomerId int,
						@CustomerName nchar(50),
						@DocketNum nchar(50),
						@CustomerRefNum nchar(50),
						@PaymentType int,
						@CurrentStoresQty float,
						@PriceCharged float,
						@SupplierId int =0,
						@LocationId int =0,
						@VatCharged float =0,
						@DeliveryCustomerId int =0,
						@UserId int =0,
						@SalesOrderNum nvarchar(50)= " ",
						@SalesOrderItemId int =0,
						@SyncId nvarchar(50)=" ",
					   	@Comment nchar(50)=" "


AS
insert into trc_Transactions
(
	ProductId,
	TraceCodeId,
	Driver,
	DateOfTransaction,
	TransactionType,
	Quantity,
	Weight,
	PONumber,
	CustomerId,
	CustomerName,
	DocketNum,
	CustomerRefNum,
	PaymentType,
	CurrentStoresQty,
	PriceCharged,
	SupplierId,	
	LocationId,
	VatCharged,
	DeliveryCustomerId,
	UserId,
	SalesOrderNum, 
	SalesOrderItemId,
	 SyncId,
	Comment
)
values
(
	@ProductId,
	@TraceCodeId,
	@Driver,
	@DateOfTransaction,
	@TransactionType,
	@Quantity,
	@Weight,
	@PONumber,
	@CustomerId,
	@CustomerName,
	@DocketNum,
	@CustomerRefNum,
	@PaymentType,
	@CurrentStoresQty,
	@PriceCharged,
	@SupplierId,
	@LocationId,
	@VatCharged,
	@DeliveryCustomerId,
	@UserId,
	@SalesOrderNum,
	@SalesOrderItemId,
	@SyncId,
	@Comment )
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

