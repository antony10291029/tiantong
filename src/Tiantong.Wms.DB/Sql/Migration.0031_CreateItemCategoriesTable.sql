create table if not exists item_categories (
  id serial not null primary key,
  warehouse_id int not null,
  name varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean not null,
  unique(warehouse_id, name)
);
