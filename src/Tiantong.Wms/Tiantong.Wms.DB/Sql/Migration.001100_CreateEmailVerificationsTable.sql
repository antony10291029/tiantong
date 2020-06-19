create table if not exists email_verifications (
  id serial not null primary key,
  email varchar(255) not null,
  code varchar(255) not null,
  is_verified boolean not null,
  expired_at timestamp(0) not null
);
