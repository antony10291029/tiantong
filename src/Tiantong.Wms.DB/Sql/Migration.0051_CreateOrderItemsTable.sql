create table if not exists order_items (
  order_id int not null,
  item_id int not null,
  order_item_id int not null,
  order_quantity int not null,
  actual_quantity int not null,
  primary key(order_id, item_id)
);
