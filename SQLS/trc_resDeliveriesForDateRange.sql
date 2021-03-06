if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resDeliveriesForDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resDeliveriesForDateRange]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE trc_resDeliveriesForDateRange	@StartDate datetime,
								@EndDate datetime
AS

select distinct DocketNum  from trc_transactions where DateOfTransaction >= @StartDate and 
DateOfTransaction < @EndDate +1
AND TransactionType  <> 0
AND DocketNum is not null
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

