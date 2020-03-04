create table if not exists locations (
  id serial not null primary key,
  warehouse_id int not null,
  area_id int not null,
  number varchar(255) not null,
  name varchar(255) not null,
  comment varchar(255) not null,
  total_area varchar(255) not null,
  total_volume varchar(255) not null,
  is_enabled boolean not null default false,
  unique(area_id, number)
);
