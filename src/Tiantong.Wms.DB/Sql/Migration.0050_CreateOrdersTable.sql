create table if not exists orders (
  id serial not null primary key,
  warehouse_id int not null,
  number varchar(255) not null,
  operator_id int not null,
  type varchar(255) not null,
  order_category_id int not null,
  created_at timestamp(0) not null,
  finished_at timestamp(0) not null,
  unique(warehouse_id, number)
);
