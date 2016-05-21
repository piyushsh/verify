if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_updOrderDeliveryCust]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_updOrderDeliveryCust]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE tls_updOrderDeliveryCust	
	@DeliveryCustomerId int,
	@SalesOrderId int
as
begin update tls_SalesOrders
set DeliveryCustomerId = @DeliveryCustomerId
where SalesOrderId = @SalesOrderId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

