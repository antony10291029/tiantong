create table if not exists lifter_tasks (
  id serial not null primary key,
  task_id varchar(255) not null,
  pallet_code varchar(255) not null,
  "from" varchar(255) not null,
  "to" varchar(255) not null,
  "status" varchar(255) not null,
  imported_at timestamp(0) not null,
  exported_at timestamp(0) not null,
  taken_at timestamp(0) not null
);
