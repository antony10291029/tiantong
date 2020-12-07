create table if not exists device_errors (
  id serial not null primary key,
  device_id int not null,
  error varchar(255) not null,
  message text not null,
  error_at timestamp(0) not null,
  recovered_at timestamp(0) not null
);

create index device_errors_device_id on device_errors("device_id");
create index device_errors_error on device_errors("error");
