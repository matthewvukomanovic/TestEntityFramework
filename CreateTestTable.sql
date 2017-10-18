create database Test
go
use Test
go
create table TestTable
( id int not null primary key,
IsActive bit not null default 1);

insert into TestTable values(1,1), (2,0), (3,1), (4,1);


select * from TestTable

-- drop table TestTable
-- use master
-- drop database Test