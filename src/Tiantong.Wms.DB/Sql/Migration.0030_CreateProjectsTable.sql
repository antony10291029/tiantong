create table if not exists projects (
  id serial not null primary key,
  warehouse_id int not null,
  number varchar(255) not null,
  name varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean not null,
  due_time timestamp(0) not null,
  started_at timestamp(0) not null,
  finished_at timestamp(0) not null,
  unique(warehouse_id, number)
);
