create table if not exists lifter_runtime_tasks (
  lifter_id varchar(255) not null,
  barcode varchar(255) not null,
  lifter_task_id int not null,
  primary key(lifter_id, barcode)
);
