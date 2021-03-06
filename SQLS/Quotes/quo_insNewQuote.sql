if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[quo_insNewQuote]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[quo_insNewQuote]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE quo_insNewQuote	
	@QuoteNum int,
             @CustomerId int,
             @CustomerContactId int,
             @DeliveryCustomerId int,
	@RequestedDeliveryDate datetime, 
             @PersonLoggingQuote int,
	@Comment nvarchar(500),
	@DateCreated  datetime, 
	@CustomerPO nvarchar(50),
	@PersonLoggingName nvarchar(50),
	@TimeCreated nvarchar(50),
	@WorkflowId int,
	@Route nvarchar(50)



AS
insert into quo_Quotes
(
	QuoteNum,
             CustomerId,
             CustomerContactId,
             DeliveryCustomerId,
	RequestedDeliveryDate, 
             PersonLoggingQuote,
	Comment,
	DateCreated, 
	CustomerPO,
	PersonLoggingName,
	Status,
	TimeCreated,
	WorkflowId,
	Route

)
values
(
	@QuoteNum,
             @CustomerId,
             @CustomerContactId,
             @DeliveryCustomerId,
	@RequestedDeliveryDate, 
             @PersonLoggingQuote,
	@Comment,
	@DateCreated, 
	@CustomerPO,
	@PersonLoggingName,
	'Open - New',
	@TimeCreated,
	@WorkflowId,
	@Route
)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

