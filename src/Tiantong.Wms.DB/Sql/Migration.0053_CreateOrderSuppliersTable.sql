create table if not exists order_suppliers (
  "key" int not null,
  order_id int not null,
  supplier_id int not null,
  order_item_ids int[] not null,
  primary key("key", order_id, supplier_id)
);
