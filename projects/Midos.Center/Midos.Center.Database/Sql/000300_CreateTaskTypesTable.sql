create table if not exists task_types (
  "id" bigserial not null primary key,
  "key" varchar(255) not null unique,
  "name" varchar(255) not null,
  "data" text not null,
  "comment" varchar(255) not null
);
