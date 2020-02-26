create table if not exists items (
  id serial not null primary key,
  repository_id int not null,
  name varchar(255) not null,
  model varchar(255) not null,
  unit_of_measurement varchar(255) not null,
  quantity real not null,
  created_at timestamp(0) not null
);
