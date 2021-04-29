create table if not exists "AgcTaskTypes" (
  "Id" bigserial not null primary key,
  "Key" varchar(255) not null unique,
  "Name" varchar(255) not null,
  "Method" varchar(255) not null,
  "Webhook" varchar(255) not null
);
