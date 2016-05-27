if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_resOpenOrdersforCustomer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_resOpenOrdersforCustomer]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE tls_resOpenOrdersforCustomer	@CustomerId int
AS
select * from tls_SalesOrders where CustomerId = @CustomerId and  status LIKE 'Open%'  order by SalesOrderId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

