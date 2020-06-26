create table if not exists verify_emails (
  id serial not null primary key,
  email varchar(255) not null,
  code varchar(255) not null,
  is_verified boolean not null,
  expired_at timestamp(0) not null
);

create table if not exists reset_passwords (
  id serial not null primary key,
  user_id int not null,
  verify_email_id int not null
);
