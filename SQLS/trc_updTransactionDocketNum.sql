if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_updTransactionDocketNum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_updTransactionDocketNum]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE trc_updTransactionDocketNum	@DocketNumNew nchar(50),
							@DocketNum nchar(50),
							@OrderNum nchar(50)
AS

begin update trc_Transactions
set DocketNum = @DocketNumNew
where SalesOrderNum = @OrderNum
AND 
DocketNum = @DocketNum
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

