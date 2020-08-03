create table if not exists logs (
  id serial not null primary key,
  name varchar(255) not null,
  email varchar(255) not null,
  password varchar(255) not null,
  avatar_url varchar(255) not null,
  is_enabled boolean not null,
  created_at timestamp(0) not null
);
