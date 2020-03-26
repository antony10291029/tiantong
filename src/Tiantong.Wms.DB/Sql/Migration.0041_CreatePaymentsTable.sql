create table if not exists payments (
  id serial not null primary key,
  amount int not null,
  order_type varchar(255) not null,
  order_number varchar(255) not null,
  comment varchar(255) not null,
  is_paid boolean not null,
  due_time timestamp(0) not null,
  paid_at timestamp(0) not null
);
