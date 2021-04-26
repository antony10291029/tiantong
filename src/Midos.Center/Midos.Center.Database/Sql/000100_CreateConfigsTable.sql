create table if not exists configs (
  "key" varchar(255) not null primary key,
  "value" varchar(255) not null,
  "updated_at" timestamp(0) not null
);
