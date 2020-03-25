create table if not exists purchase_orders (
  id serial not null primary key,
  warehouse_id int not null,
  number varchar(255) not null,
  applicant_id int not null,
  department_id int not null,
  operator_id int not null,
  supplier_id int not null,
  status varchar(255) not null,
  comment varchar(255) not null,
  due_time timestamp(0) not null,
  created_at timestamp(0) not null,
  finished_at timestamp(0) not null,
  item_ids int[] not null,
  payment_ids int[] not null,
  unique(warehouse_id, number)
);
