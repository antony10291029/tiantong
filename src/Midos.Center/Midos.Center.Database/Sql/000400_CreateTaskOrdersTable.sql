create table if not exists "TaskOrders" (
  "Id" bigserial not null primary key,
  "TypeId" bigint not null,
  "Code" varchar(255),
  "Status" varchar(255) not null,
  "Data" text not null,
  "CreatedAt" timestamp(0) not null,
  "StartedAt" timestamp(0) not null,
  "ClosedAt" timestamp(0) not null,

  unique("Code", "TypeId")
);
