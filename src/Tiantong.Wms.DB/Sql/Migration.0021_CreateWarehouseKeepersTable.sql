create table if not exists warehouse_keepers (
  warehouse_id int not null,
  keeper_user_id int not null,
  role varchar(255) not null default 'admin',
  is_enabled boolean not null default false,
  primary key(warehouse_id, keeper_user_id, role)
);
