create table if not exists purchase_order_items (
  id serial not null primary key,
  good_id int not null,
  item_id int not null,
  price float not null,
  quantity int not null,
  delivery_time varchar(255) not null,
  tax_number varchar(255) not null,
  tax_name varchar(255) not null,
  tax_specification varchar(255) not null,
  tax_type varchar(255) not null,
  tax_rate float not null,
  item_project_ids int[] not null,
);
