if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_updInvoiceNumForDocketAndOrderNum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_updInvoiceNumForDocketAndOrderNum]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE trc_updInvoiceNumForDocketAndOrderNum	@DocketNum nchar(50),
									@SalesOrderNum nchar(50),
								@InvoiceNum nchar(50)
AS
begin update trc_Transactions
set InvoiceNum = @InvoiceNum 
where DocketNum = @DocketNum and SalesOrderNum=@SalesOrderNum
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

