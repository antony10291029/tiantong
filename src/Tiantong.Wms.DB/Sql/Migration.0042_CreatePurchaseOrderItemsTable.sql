create table if not exists purchase_order_items (
  id serial not null primary key,
  order_id int not null,
  good_id int not null,
  item_id int not null,
  invoice_id int not null,
  "index" int not null,
  price float not null,
  quantity int not null,
  comment varchar(255) not null,
  delivery_cycle varchar(255) not null,
  arrived_at timestamp(0) not null
);
