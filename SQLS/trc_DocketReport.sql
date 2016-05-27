/*   21 September 2009 12:49:55   User: sa   Server: JUDYVOSTRO   Database: SeerysNew   Application: MS SQLEM - Data Tools*/BEGIN TRANSACTION
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
ALTER TABLE dbo.trc_DocketReport ADD
	UserID int NULL,
	SalesOrderID int NULL,
	ExtraData1 nvarchar(100) NULL,
	ExtraData2 nvarchar(100) NULL,
	ExtraData3 nvarchar(100) NULL,
	ExtraData4 nvarchar(100) NULL,
	ExtraData5 nvarchar(100) NULL,
	ExtraData6 nvarchar(100) NULL,
	ExtraData7 nvarchar(100) NULL,
	ExtraData8 nvarchar(100) NULL,
	ExtraData9 nvarchar(100) NULL,
	ExtraData10 nvarchar(100) NULL
GO
COMMIT
