create table if not exists order_suppliers (
  order_id int not null,
  supplier_id int not null,
  primary key(order_id, supplier_id)
);
