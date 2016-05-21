if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_delDocketReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_delDocketReport]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE trc_delDocketReport   @UserId int = 0

AS
if @UserId = 0 
	Delete  from trc_DocketReport
else
	Delete from trc_DocketReport where UserId = @UserId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

