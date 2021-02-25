create table if not exists logs (
  id serial not null primary key,
  "key" varchar(255) not null,
  type varchar(255) not null,
  message text not null,
  created_at timestamp(0) not null
);

create index logs_type on logs(type);
create index logs_key on logs("key");
