create table if not exists items (
  id serial not null primary key,
  warehouse_id int not null,
  good_id int not null,
  number varchar(255),
  name varchar(255) not null,
  unit varchar(255) not null,
  is_enabled boolean not null,
  stock_ids int[] not null,
  unique(warehouse_id, number)
);
