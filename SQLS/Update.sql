/*   24 August 2009 11:13:42   User: sa   Server: JUDYVOSTRO   Database: SeerysNew   Application: MS SQLEM - Data Tools*/BEGIN TRANSACTION
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
ALTER TABLE dbo.tls_SalesOrders ADD
	Type nvarchar(50) NULL
GO
COMMIT


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PBS_sp_ShowPendingMergeChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PBS_sp_ShowPendingMergeChanges]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROC PBS_sp_ShowPendingMergeChanges (@p_articlename sysname =
'%')

AS

BEGIN

-- Fetch the rowguidcol of the article
declare @ROWGUIDCol sysname
declare @sql varchar(8000)
declare @articlename sysname

create table #t
(
Publisher varchar(50),
Publication varchar(50),
Article varchar(50),
Type varchar(15),
PrimaryKey uniqueidentifier
)

-- Declare cursor to report for all articles
DECLARE PBS_cs_mergearticles CURSOR LOCAL FAST_FORWARD FOR
SELECT ltrim(rtrim(name)) FROM sysmergearticles where name like
@p_articlename

OPEN PBS_cs_mergearticles

FETCH NEXT FROM PBS_cs_mergearticles INTO @articlename

WHILE @@FETCH_STATUS = 0 BEGIN

select @ROWGUIDCol = c.[name]
from syscolumns c
where c.id = object_id(@articlename)
and columnproperty(object_id(@articlename), c.[name],
'IsRowGUIDCol') = 1

if @ROWGUIDCol is null
return -- Can't determine the rowguidcol so return null

set @sql =
'SELECT distinct ' +
' sysmergepublications.publisher AS Publisher, ' +
' sysmergepublications.name AS Publication, ' +
' sysmergearticles.name AS Article, ' +
' ''Ins/Upd'' as Type, ' +
' MSmerge_contents.rowguid AS PrimaryKey ' +
-- '[' + @articlename + '].* ' +
'FROM ' +
' MSmerge_contents with (nolock) ' +
' INNER JOIN [' + @articlename + '] with (nolock) ' +
' ON MSmerge_contents.rowguid = [' + @articlename + '].' +
@ROWGUIDCol +
' INNER JOIN sysmergearticles with (nolock) ' +
' ON MSmerge_contents.tablenick = sysmergearticles.nickname ' +
' INNER JOIN sysmergesubscriptions with (nolock) ' +
' ON sysmergearticles.pubid = sysmergesubscriptions.partnerid ' +
' AND sysmergesubscriptions.db_name = db_name() ' +
' INNER JOIN sysmergepublications with (nolock) ' +
' ON sysmergesubscriptions.pubid = sysmergepublications.pubid ' +
' WHERE ' +
' sysmergearticles.name = ''' + @articlename + ''' ' +
' AND sysmergearticles.gen_cur = MSmerge_contents.generation ' +

' UNION ALL ' +

' SELECT distinct ' +
' sysmergepublications.publisher, ' +
' sysmergepublications.name, ' +
' sysmergearticles.name, ' +
' ''Del'', ' +
' MSmerge_tombstone.rowguid ' +
-- '[' + @articlename + '].* ' +
' FROM ' +
' MSmerge_tombstone with (nolock) ' +
' INNER JOIN sysmergearticles with (nolock) ' +
' ON MSmerge_tombstone.tablenick = sysmergearticles.nickname ' +
' INNER JOIN sysmergesubscriptions with (nolock) ' +
' ON sysmergearticles.pubid = sysmergesubscriptions.partnerid ' +
' AND sysmergesubscriptions.db_name = db_name() ' +
' INNER JOIN sysmergepublications with (nolock) ' +
' ON sysmergesubscriptions.pubid = sysmergepublications.pubid ' +
' LEFT OUTER JOIN [' + @articlename + ']' +
' ON MSmerge_tombstone.rowguid = [' + @articlename + '].' +
@ROWGUIDCol +
' WHERE sysmergearticles.name = ''' + @articlename + '''' +
' AND sysmergearticles.gen_cur = MSmerge_tombstone.generation '

INSERT INTO #t
Exec (@sql)


FETCH NEXT FROM PBS_cs_mergearticles INTO @articlename

END

CLOSE PBS_cs_mergearticles
DEALLOCATE PBS_cs_mergearticles

Select * from #t order by publisher, publication, article, type,
primarykey

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[trc_resDeliveriesForDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[trc_resDeliveriesForDateRange]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE trc_resDeliveriesForDateRange	@StartDate datetime,
								@EndDate datetime
AS

select distinct DocketNum  from trc_transactions where DateOfTransaction >= @StartDate and 
DateOfTransaction < @EndDate +1
AND TransactionType  <> 0
AND DocketNum is not null
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tls_updOrderType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[tls_updOrderType]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE tls_updOrderType	@SalesOrderId int,
						@OrderType nchar(50)
AS
begin update tls_SalesOrders 
set Type = @OrderType where SalesOrderId = @SalesOrderId
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


insert into wfo_Config (DataItemName, DataItemValue) values ('DisplayTeleProdSearch', 'Yes')
go

insert into wfo_Config (DataItemName, DataItemValue) values ('DifferentSalesOrderTypes', 'Yes')
go

insert into wfo_Config (DataItemName, DataItemValue) values ('CalcWeightFromQty', 'Yes')
go




