create table if not exists items (
  id serial not null primary key,
  warehouse_id int not null,
  number varchar(255) not null,
  category_ids int[] not null,
  name varchar(255) not null,
  specification varchar(255) not null,
  reference_purchase_price float not null,
  comment varchar(255) not null,
  is_enabled boolean not null,
  created_at timestamp(0) not null,
  unique(warehouse_id, number)
);
