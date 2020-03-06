create table if not exists stock_records (
  id serial not null primary key,
  warehouse_id int not null,
  stock_id int not null,
  order_id int not null,
  order_item_id int not null,
  quantity int not null,
  created_at timestamp(0) not null
);
