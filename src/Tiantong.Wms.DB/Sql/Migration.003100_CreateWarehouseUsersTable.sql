create table if not exists warehouse_users (
  id serial not null primary key,
  warehouse_id int not null,
  user_id int not null,
  unique(warehouse_id, user_id)
);