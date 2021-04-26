create table if not exists apps (
  "id" bigserial not null primary key,
  "class" varchar(255) not null,
  "key" varchar(255) not null unique,
  "name" varchar(255) not null,
  "url" varchar(255) not null,
  "created_at" timestamp(0) not null
);
