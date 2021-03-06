if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_updSalesOrderStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_updSalesOrderStatus]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE tls_updSalesOrderStatus	@SalesOrderId int,
							@Status nchar(50)
AS

begin update tls_SalesOrders
set Status = @Status
where SalesOrderId = @SalesOrderId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

