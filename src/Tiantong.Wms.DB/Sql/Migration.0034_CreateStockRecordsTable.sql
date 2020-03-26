create table if not exists stock_records (
  id serial not null primary key,
  stock_id int not null,
  order_type varchar(255) not null,
  order_number varchar(255) not null,
  quantity int not null,
  created_at timestamp(0) not null
);
