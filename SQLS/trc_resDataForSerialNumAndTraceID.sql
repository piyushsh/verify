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

