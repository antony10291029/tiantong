create table if not exists "SubtaskTypes" (
  "Id" bigserial not null primary key,
  "Key" varchar(255) not null,
  "Index" int not null,
  "TypeId" bigint not null,
  "SubtypeId" bigint not null,
  unique("Key", "TypeId")
);
