if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_updOrderType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_updOrderType]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE tls_updOrderType	@SalesOrderId int,
						@OrderType nchar(50)
AS
begin update tls_SalesOrders 
set Type = @OrderType where SalesOrderId = @SalesOrderId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

