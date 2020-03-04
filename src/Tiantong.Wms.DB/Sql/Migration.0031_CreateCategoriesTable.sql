create table if not exists categories (
  id serial not null primary key,
  warehouse_id int not null,
  type varchar(255) not null,
  name varchar(255) not null,
  is_enabled boolean not null default false,
  unique(warehouse_id, type, name)
);
