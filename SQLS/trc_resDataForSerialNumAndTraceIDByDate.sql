if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resDataForSerialNumAndTraceIDByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resDataForSerialNumAndTraceIDByDate]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE trc_resDataForSerialNumAndTraceIDByDate	@SerialNum nvarchar(50),
								@TraceCodeId int
AS

select *
from trc_Transactions where serialNum = @SerialNum and TraceCodeId = @TraceCodeId
order by DateOfTransaction desc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

