create table if not exists devices (
  id serial not null primary key,
  name varchar(255) not null,
  number varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean not null
);

create table if not exists device_states (
  id serial not null primary key,
  device_id integer not null,
  state varchar(255),
  mode varchar(255),
  position varchar(255),
  message varchar(255),
  created_at timestamp not null
);
