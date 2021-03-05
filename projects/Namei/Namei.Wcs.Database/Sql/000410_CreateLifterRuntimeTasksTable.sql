create table if not exists lifter_runtime_tasks (
  barcode varchar(255) not null primary key,
  lifter_task_id int not null
);
