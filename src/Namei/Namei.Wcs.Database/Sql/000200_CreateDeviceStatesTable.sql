create table if not exists device_states (
  id serial not null primary key,
  type varchar(255) not null,
  device_id int not null,
  state varchar(255) not null,
  started_at timestamp(0) not null,
  ended_at timestamp(0) not null
);

create index device_type_device_id on device_states (type, device_id);
