create table if not exists warehouses (
  id serial not null primary key,
  owner_user_id int not null,
  number varchar(255) not null,
  name varchar(255) not null,
  address varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean default false,
  created_at timestamp(0) not null,
  unique(owner_user_id, number)
);
