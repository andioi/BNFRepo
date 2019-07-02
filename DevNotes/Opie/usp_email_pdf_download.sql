USE [CBASSID]
GO

CREATE PROCEDURE [dbo].[USP_EMAIL_PDF_DOWNLOAD] 
	@appid VARCHAR(20),
	@userid VARCHAR(150)
AS

DECLARE @reffnumber VARCHAR(20),
		@TO VARCHAR(1000),
		@TO_TEMP varchar(500),
		@CC VARCHAR(1000),
		@SUBJECT VARCHAR(200),
		@BODY VARCHAR(1000),
		@ATTACHPATH VARCHAR(250),
		@USERNAME VARCHAR(150),
		@USERNAME_INPUTTER VARCHAR(150)


-- @reffnumber
select top 1 @reffnumber=ISNULL(reffnumber,'') from slik_applicant where appid = @appid

-- TO
set @TO = ''
SELECT TOP 1 @TO = ISNULL(SU_EMAIL,''), @USERNAME_INPUTTER = SU_FULLNAME 
FROM SCALLUSER where USERID = @userid

-- CC other
set @CC = ''
DECLARE CRDATA CURSOR FOR
select su_email from scalluser where groupid = 'GCC1'
OPEN CRDATA
FETCH NEXT FROM CRDATA INTO @TO_TEMP
WHILE @@FETCH_STATUS = 0 
BEGIN
	IF CHARINDEX(@TO_TEMP,@CC) = 0 AND @TO_TEMP <> '' SET @CC = @TO_TEMP+isnull(';'+@CC,'')
	IF RIGHT(@CC,1)=';' SET @CC = SUBSTRING(@CC,1,LEN(@CC)-1)
	FETCH NEXT FROM CRDATA INTO @TO_TEMP
END
CLOSE CRDATA
DEALLOCATE CRDATA

-- nama nasabah
declare @custname varchar(100)
select @custname = cust_name from apprequest cp where requestid = @reffnumber

SET @SUBJECT = @USERNAME_INPUTTER + ' telah mendownload iDEB pdf debitur '+@custname
SET @BODY = 'Dear All,<br><br>'
SET @BODY = @BODY + 'Diinformasikan bahwa iDEB pdf a.n '+ @custname + ' telah didownload oleh ' + @USERNAME_INPUTTER + '. <br><br>Reff Number : '+@reffnumber+'.<br><br>'

if @SUBJECT is not null and ISNULL(@TO,'') <> ''
begin
	exec USP_EMAILNOTIFIER_INSERT @reffnumber, @to, @cc, @subject, @BODY, '', ''
end