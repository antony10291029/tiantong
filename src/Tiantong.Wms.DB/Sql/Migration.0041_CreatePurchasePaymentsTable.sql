create table if not exists purchase_payments (
  id serial not null primary key,
  order_id int not null,
  "index" int not null,
  amount float not null,
  comment varchar(255) not null,
  is_paid boolean not null,
  due_time timestamp(0) not null,
  paid_at timestamp(0) not null
);
