if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_insNewSalesOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_insNewSalesOrder]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE tls_insNewSalesOrder	
	@CustomerId int,
	@DeliveryCustomerId int,
	@SalesOrderNum char(10),
	@CustomerContactId int,
	@RequestedDeliveryDate datetime,
	@PersonLoggingOrder int,
	@Comment nchar(500),
	@DateCreated datetime,
	@TimeCreated nvarchar(50),
	@IsBackOrder bit = 0,
	@ParentId int, @WorkflowId int
AS
insert into tls_SalesOrders
(
	CustomerId,
	DeliveryCustomerId,
	SalesOrderNum,
	CustomerContactId,
	RequestedDeliveryDate,
	PersonLoggingOrder,
	Comment,
	DateCreated,
	TimeCreated,
	IsBackOrder,
	Status,
	ParentSalesOrderId,
	WorkflowId
)
values
(
	@CustomerId,
	@DeliveryCustomerId,
	@SalesOrderNum,
	@CustomerContactId,
	@RequestedDeliveryDate,
	@PersonLoggingOrder,
	@Comment,
	@DateCreated,
	@TimeCreated,
	@IsBackOrder,
	'Open - New',
	@ParentId,@WorkflowId

)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

