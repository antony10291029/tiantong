create table if not exists lifter_tasks (
  id serial not null primary key,
  lifter_id varchar(255) not null,
  floor varchar(255) not null,
  destination varchar(255) not null,
  barcode varchar(255) not null,
  task_code varchar(255) not null,
  "status" varchar(255) not null,
  created_at timestamp(0) not null,
  imported_at timestamp(0) not null,
  exported_at timestamp(0) not null,
  taken_at timestamp(0) not null
);
