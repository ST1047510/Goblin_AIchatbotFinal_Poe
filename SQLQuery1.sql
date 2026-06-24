Create Database User_tasks;

use User_tasks;

Create Table usertasks(
task_id int primary key identity(1,1),
task_name VarChar(20) not null,
task_description VarChar(20) not null,
task_status VarChar (20) not null,
task_due_date Varchar(20) not null,
);

alter table usertasks
alter Column task_description VarChar(max);

select * from usertasks;