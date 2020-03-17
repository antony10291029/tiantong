create table if not exists order_items (
  id serial not null primary key,
  order_id int not null,
  "key" int not null,
  item_id int not null,
  price float not null,
  expected_quantity int not null,
  actual_quantity int not null,
  unique(order_id, "key")
);
