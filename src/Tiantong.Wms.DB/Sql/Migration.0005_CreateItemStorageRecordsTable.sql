create table if not exists item_storage_records (
  id serial not null primary key,
  item_id int not null,
  operator_id int not null,
  quantity real not null,
  method varchar(255) not null,
  created_at timestamp(0) not null
);
