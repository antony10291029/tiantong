create table if not exists email_verifications (
  id serial not null primary key,
  address varchar(255) not null,
  "key" varchar(255) not null,
  code varchar(255) not null,
  error_count integer not null,
  expired_at timestamp(0) not null
);
