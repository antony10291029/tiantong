create table if not exists orders (
  id serial not null primary key,
  warehouse_id int not null,
  number varchar(255) not null,
  type varchar(255) not null,
  category_id int not null,
  operator_id int not null,
  status varchar(255) not null,
  comment varchar(255) not null,
  due_time timestamp(0) not null,
  created_at timestamp(0) not null,
  finished_at timestamp(0) not null,
  unique(warehouse_id, number)
);
