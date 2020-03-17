create table if not exists order_suppliers (
  id serial not null primary key,
  "key" int not null,
  order_id int not null,
  supplier_id int not null,
  item_keys int[] not null,
  unique(order_id, supplier_id, "key")
);
