create table if not exists password_resets (
  id serial not null primary key,
  user_id int not null,
  email_verification_id int not null
);
