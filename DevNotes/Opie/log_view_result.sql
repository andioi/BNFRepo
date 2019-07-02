USE [CBASSID]
GO

CREATE TABLE [dbo].[log_view_result](
	[log_seq] [int] IDENTITY(1,1) NOT NULL,
	[log_date] [datetime] NULL,
	[userid] [varchar](150) NULL,
	[host] [varchar](50) NULL,
	[reffnumber] [varchar](20) NULL,
 CONSTRAINT [PK_log_view_result] PRIMARY KEY CLUSTERED 
(
	[log_seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE PROCEDURE [dbo].[LOG_VIEW_RESULT_INSERT] 
	@userid VARCHAR(150),
	@host VARCHAR(50),
	@appid VARCHAR(20)

AS

DECLARE @reffnumber VARCHAR(20)

select top 1 @reffnumber=ISNULL(reffnumber,'') from slik_applicant where appid = @appid

if not exists (select top 1 1 from log_view_result where userid=@userid and reffnumber=@reffnumber and CONVERT(date, log_date)=CONVERT(date, getdate()))
begin
	insert into log_view_result (log_date, userid, host, reffnumber) values (getdate(), @userid, @host, @reffnumber)
end

GO

create view [dbo].[VW_LOG_VIEW_RESULT]
AS
select
	a.userid
	,a.log_date
	,a.host
	,a.reffnumber
	,b.cust_name
	,b.ktp
	,c.su_fullname
from
	log_view_result a
left join
	slik_applicant b
on
	a.reffnumber = b.reffnumber
left join
	scalluser c
on
	a.userid = c.userid

GO

insert into pivotlist values ('RPT_LOGVIEWRESULT','LOG VIEW RESULT','@[VW_LOG_VIEW_RESULT|log_date]:DATE TIME,@[VW_LOG_VIEW_RESULT|userid]:USER ID,@[VW_LOG_VIEW_RESULT|su_fullname]:USER NAME,@[VW_LOG_VIEW_RESULT|host]:IP ADDRESS,@[VW_LOG_VIEW_RESULT|reffnumber]:REFFNUMBER,@[VW_LOG_VIEW_RESULT|cust_name]:DEBTOR NAME,@[VW_LOG_VIEW_RESULT|ktp]:KTP/NIK',NULL,NULL,'@[VW_LOG_VIEW_RESULT|log_date]:DATE TIME:dr,@[VW_LOG_VIEW_RESULT|userid]:USER ID,@[VW_LOG_VIEW_RESULT|reffnumber]:REFFNUMBER,@[VW_LOG_VIEW_RESULT|cust_name]:DEBTOR NAME',NULL)
GO

insert into reportset values ('RPT_LOGPARAM','RPT_LOGVIEWRESULT')
GO