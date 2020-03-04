create table if not exists items (
  id serial not null primary key,
  warehouse_id int not null,
  item_category_id int not null,
  number varchar(255) not null,
  name varchar(255) not null,
  specification varchar(255) not null,
  comment varchar(255) not null,
  picture_url varchar(255) not null,
  unique(warehouse_id, number)
);
