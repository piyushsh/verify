if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_insSupplierDispatchTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_insSupplierDispatchTransaction]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE trc_insSupplierDispatchTransaction   @ProductId int,
						@TraceCodeId int,
						@Driver nchar(50),
						@DateOfTransaction datetime,
					    	@Quantity int,
						@TransactionType int,
						@PONumber nchar(50),
						@SupplierId int,
						@SupplierName nchar(50),
						@Weight float,
						@DocketNum nchar(50),
						@CurrentStoresQty float,
						@PriceCharged float,
						@LocationId int =0,
						@VatCharged float =0,
						@UserId int =0,
						@SyncId nvarchar(50)=''


AS
insert into trc_Transactions
(
	ProductId,
	TraceCodeId,
	Driver,
	DateOfTransaction,
	Quantity,
	TransactionType,
	PONumber,
	SupplierId,
	CustomerName,
	Weight,
	DocketNum,
	CurrentStoresQty,
	PriceCharged,
	LocationId,
	VatCharged,
	UserId,
	SyncId
)
values
(
	@ProductId,
	@TraceCodeId,
	@Driver,
	@DateOfTransaction,
	@Quantity,
	@TransactionType,
	@PONumber,
	@SupplierId,
	@SupplierName,
	@Weight,
	@DocketNum,
	@CurrentStoresQty,
	@PriceCharged,
	@LocationId,
	@VatCharged,
	@UserId,
	@SyncId  )
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

