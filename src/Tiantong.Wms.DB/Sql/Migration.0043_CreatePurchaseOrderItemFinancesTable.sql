create table if not exists purchase_order_item_finances (
  id int not null primary key,
  name varchar(255) not null,
  specification varchar(255) not null,
  unit varchar(255) not null,
  quantity int not null,
  price float not null,
  amount float not null,
  tax_rate float not null,
  tax_amount float not null,
  invoice_number varchar(255) not null,
  invoice_type varchar(255) not null
);
