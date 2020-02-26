create table if not exists repositories (
  id serial not null primary key,
  name varchar(255) not null,
  created_at timestamp(0) not null
);
