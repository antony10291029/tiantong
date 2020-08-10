create table if not exists jobs (
  id serial not null primary key,
  name varchar(255) not null,
  interval int not null,
  is_enable boolean not null,
  count int not null,
  executed_at timestamp(0) not null
);
