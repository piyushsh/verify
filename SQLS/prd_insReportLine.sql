USE [Seerys]
GO
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



