BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.wfo_UserConfig
	(
	ConfigID int NOT NULL IDENTITY (1, 1),
	DataItemName nvarchar(100) NULL,
	DataItemValue nvarchar(300) NULL
	)  ON [PRIMARY]
GO
COMMIT



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wfo_resGetUserConfigItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wfo_resGetUserConfigItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wfo_updSetUserConfigItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wfo_updSetUserConfigItem]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO





CREATE PROCEDURE wfo_resGetUserConfigItem
	@ConfigItemName nvarchar(100)

AS
	SELECT * FROM wfo_UserConfig WHERE DataItemName = @ConfigItemName



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO




CREATE PROCEDURE wfo_updSetUserConfigItem
	@ConfigItemName nvarchar(100), @ConfigItemValue nvarchar(300)

AS
	DECLARE @tempIndex INT

	set @tempIndex = (SELECT Count(*) FROM wfo_UserConfig WHERE DataItemName= @ConfigItemName)
	if @tempIndex = 0
		INSERT INTO  wfo_UserConfig (DataItemName, DataItemValue) VALUES(@ConfigItemName, @ConfigItemValue)
	else
		UPDATE wfo_UserConfig SET DataItemValue= @ConfigItemValue WHERE DataItemName=@ConfigItemName



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

