if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_insDocketReportRowExtended]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_insDocketReportRowExtended]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE trc_insDocketReportRowExtended 	@ProductCode nchar(50),
						@ProductDescription nchar(50),
						@TraceCodeDesc nchar(50) ,
						@DateDelivery datetime ,
						@Weight float,
						@Quantity float,
						@PriceCharged float,
						@IsTotalsRow bit,
						@TotalCost float,
						@DocketNum nchar(50),
						@VatCharge float,
						@TotalVatAmount float,
						@TotalNetAmount float,
						@OrderNum int,
						@UserID int,
						@SalesOrderID int,
 						@ExtraData1 nchar(100),
 						@ExtraData2 nchar(100),
 						@ExtraData3 nchar(100),
 						@ExtraData4 nchar(100),
 						@ExtraData5 nchar(100),
 						@ExtraData6 nchar(100),
 						@ExtraData7 nchar(100),
 						@ExtraData8 nchar(100),
 						@ExtraData9 nchar(100),
 						@ExtraData10 nchar(100)
						

AS

insert into trc_DocketReport
(
	ProductCode,
	ProductDescription,
	TraceCodeDesc,
	DateDelivery,
	Weight,
	Quantity,
	PriceCharged,
	IsTotalsRow,
	TotalCost,
	DocketNum,
	VatCharge,
	TotalVatAmount,
	TotalNetAmount,
	OrderNum, 
	UserID,
	SalesOrderID,
	ExtraData1,
 	ExtraData2,
 	ExtraData3,
 	ExtraData4,
 	ExtraData5,
 	ExtraData6,
 	ExtraData7,
 	ExtraData8,
 	ExtraData9,
 	ExtraData10 
						
)
values
(
	@ProductCode,
	@ProductDescription,
	@TraceCodeDesc,
	@DateDelivery,
	@Weight,
	@Quantity,
	@PriceCharged,
	@IsTotalsRow,
	@TotalCost,
	@DocketNum,
	@VatCharge,
	@TotalVatAmount,
	@TotalNetAmount,
	@OrderNum, 
	@UserID,
	@SalesOrderID, 
	@ExtraData1,
 	@ExtraData2,
 	@ExtraData3,
 	@ExtraData4,
 	@ExtraData5,
 	@ExtraData6,
 	@ExtraData7,
 	@ExtraData8,
 	@ExtraData9,
 	@ExtraData10 
						
)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

