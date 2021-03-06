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

