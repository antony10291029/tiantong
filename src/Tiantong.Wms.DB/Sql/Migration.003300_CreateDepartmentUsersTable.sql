create table if not exists department_users (
  id serial not null primary key,
  department_id int not null,
  user_id int not null,
  role varchar(255) not null,
  unique(department_id, user_id, role)
);
