create table if not exists devices (
  id serial not null primary key,
  "key" varchar(255) not null,
  name varchar(255) not null,
  type varchar(255) not null,
  is_enable boolean not null,
  created_at timestamp(0) not null,
  unique("key")
);
