create table if not exists rcs_tasks (
  id serial not null primary key,
  type varchar(255) not null,
  uuid varchar(255) not null,
  device_type varchar(255) not null,
  devicec_index varchar(255) not null,
  action_task varchar(255) not null,
  src varchar(255) not null,
  dst varchar(255) not null,
  created_at timestamp(0) not null
);
