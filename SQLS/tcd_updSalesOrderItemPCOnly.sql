if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tcd_updSalesOrderItemPCOnly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tcd_updSalesOrderItemPCOnly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO







CREATE PROCEDURE tcd_updSalesOrderItemPCOnly	

	
	@ItemId int,
	@PCOnly bit
AS

begin update tcd_tblSalesOrders

set PCOnly =@PCOnly
where ItemNumber = @ItemId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

