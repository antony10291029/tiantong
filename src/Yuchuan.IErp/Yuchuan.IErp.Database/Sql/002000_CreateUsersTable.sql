create table if not exists users (
  id serial not null primary key,
  name varchar(255) not null,
  email varchar(255) not null,
  mobile varchar(255) not null,
  wechat_id varchar(255) not null,
  password varchar(255) not null,
  is_enabled boolean not null,
  is_verified boolean not null,
  created_at timestamp not null
);
