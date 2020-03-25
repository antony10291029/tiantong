create table if not exists users (
  id serial not null primary key,
  type varchar(255) not null,
  email varchar(255) not null,
  password varchar(255) not null,
  number varchar(255),
  name varchar(255) not null,
  is_enabled boolean not null,
  created_at timestamp(0) not null,
  unique(email)
);
