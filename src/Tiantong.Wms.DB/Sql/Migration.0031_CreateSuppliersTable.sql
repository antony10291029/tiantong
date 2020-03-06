create table if not exists suppliers (
  id serial not null primary key,
  warehouse_id int not null,
  name varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean not null default false,
  unique(warehouse_id, name)
);
