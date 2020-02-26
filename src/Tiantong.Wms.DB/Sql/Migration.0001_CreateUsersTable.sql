create table if not exists users (
  id serial not null primary key,
  email varchar(255) not null,
  password varchar(255) not null,
  roles varchar(255)[] not null,
  name varchar(255),
  created_at timestamp(0) not null
);
