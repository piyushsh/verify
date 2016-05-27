if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[quo_resQuoteForId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[quo_resQuoteForId]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE quo_resQuoteForId	@QuoteId int
AS
Select * from quo_Quotes where QuoteId = @QuoteId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

