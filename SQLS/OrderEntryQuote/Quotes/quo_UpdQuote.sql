if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[quo_UpdQuote]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[quo_UpdQuote]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE quo_UpdQuote	
	@QuoteId int,
             @CustomerId int,
             @CustomerContactId int,
             @DeliveryCustomerId int,
	@RequestedDeliveryDate datetime, 
             @PersonLoggingQuote int,
	@Comment nvarchar(500),
	@CustomerPO nvarchar(50),
	@PersonLoggingName nvarchar(50),
	@Status nvarchar(50),
	@Route nvarchar(50)



AS
begin update quo_Quotes

	set  CustomerId =  @CustomerId,
            	CustomerContactId =  @CustomerContactId,
            	DeliveryCustomerId =  @DeliveryCustomerId,
	RequestedDeliveryDate = @RequestedDeliveryDate, 
            	PersonLoggingQuote = @PersonLoggingQuote,
	Comment = @Comment,
	CustomerPO = @CustomerPO,
	PersonLoggingName = @PersonLoggingName,
	Status = @Status,
	Route = @Route
	where QuoteId = @QuoteId

end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

