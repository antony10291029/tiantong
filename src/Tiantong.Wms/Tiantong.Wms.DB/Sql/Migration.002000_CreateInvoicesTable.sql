create table if not exists invoices (
  id serial not null primary key,
  name varchar(255) not null,
  specification varchar(255) not null,
  unit varchar(255) not null,
  quantity int not null,
  price float not null,
  amount float not null,
  tax_rate float not null,
  tax_amount float not null,
  number varchar(255) not null,
  type varchar(255) not null
);
