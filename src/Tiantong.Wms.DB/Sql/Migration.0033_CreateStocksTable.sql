create table if not exists stocks (
  id serial not null primary key,
  warehouse_id int not null,
  good_id int not null,
  item_id int not null,
  area_id int not null,
  location_id int not null,
  quantity int not null,
  unique(warehouse_id, item_id, location_id)
);
