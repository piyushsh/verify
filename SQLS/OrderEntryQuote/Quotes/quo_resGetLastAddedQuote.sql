if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[quo_resGetLastAddedQuote]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[quo_resGetLastAddedQuote]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].[quo_resGetLastAddedQuote]
AS

select max(QuoteId) from quo_Quotes
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

