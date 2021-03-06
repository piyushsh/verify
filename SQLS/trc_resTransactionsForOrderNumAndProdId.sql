if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resTransactionsForOrderNumAndProdId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resTransactionsForOrderNumAndProdId]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE trc_resTransactionsForOrderNumAndProdId	@SalesOrderNum nvarchar(50),
									@ProductID int
AS
select * from trc_transactions 
where SalesOrderNum = @SalesOrderNum and ProductID = @ProductID  order by DateOfTransaction
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

