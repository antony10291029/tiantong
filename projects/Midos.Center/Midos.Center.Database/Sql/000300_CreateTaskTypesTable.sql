create table if not exists "TaskTypes" (
  "Id" bigserial not null primary key,
  "Key" varchar(255) not null unique,
  "Name" varchar(255) not null,
  "HasCode" boolean not null,
  "Data" text not null,
  "Comment" varchar(255) not null
);
