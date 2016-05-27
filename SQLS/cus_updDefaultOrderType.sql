if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[cus_updDefaultOrderType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[cus_updDefaultOrderType]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE cus_updDefaultOrderType	@CustomerId int,
						@DefaultOrderType nchar(50)

AS
begin update cus_CustomerDetails 
set DefaultOrderType = @DefaultOrderType where CustomerId = @CustomerId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

