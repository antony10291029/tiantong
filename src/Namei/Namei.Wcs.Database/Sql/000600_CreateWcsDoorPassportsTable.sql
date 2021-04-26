create table if not exists "WcsDoorPassports" (
  "Id" varchar(255) not null primary key,
  "ExpiredAt" timestamp(0) not null
);
