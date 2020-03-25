create table if not exists keepers (
  int serial not null primary key,
  warehouse_id int not null,
  user_id int not null,
  department_id int not null,
  role varchar(255) not null,
  is_enabled boolean not null,
  unique(warehouse_id, user_id, role)
);
