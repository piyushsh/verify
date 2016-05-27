if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tcd_resSalesOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tcd_resSalesOrder]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE tcd_resSalesOrder	@SalesOrderNum int
AS

select * from tcd_tblSalesOrders where OrderNumber = @SalesOrderNum
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

