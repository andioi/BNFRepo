use CBASSID

GO

alter table apprequest add nationality varchar(10), marital_status varchar(10)

alter table apprequestsupp add nationality varchar(10)

GO

alter table rfrelationbic add cust_type varchar(3)

GO

update rfrelationbic set cust_type = 'IND' where rel_code in ('KLG','SPS','ALS')
update rfrelationbic set cust_type = 'PSH' where rel_code in ('OBL','PEN','PES','PMS')

GO

