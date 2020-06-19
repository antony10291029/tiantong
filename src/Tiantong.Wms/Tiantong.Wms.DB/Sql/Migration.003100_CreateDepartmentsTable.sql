create table if not exists departments (
  id serial not null primary key,
  warehouse_id int not null,
  type varchar(255) not null,
  name varchar(255) not null,
  comment varchar(255) not null,
  unique(warehouse_id, name)
);
