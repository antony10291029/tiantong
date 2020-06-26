create table if not exists projects (
  id serial not null primary key,
  name varchar(255) not null,
  is_enabled boolean not null,
  created_at timestamp not null
);

create table if not exists project_users (
  id serial not null primary key,
  project_id integer not null,
  user_id integer not null,
  role varchar(255) not null
);

create table if not exists project_devices (
  id serial not null,
  project_id integer not null,
  device_id integer not null
);
