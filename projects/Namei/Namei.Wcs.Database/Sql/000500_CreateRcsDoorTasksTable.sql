create table if not exists "RcsDoorTasks" (
  "Uuid" varchar(255) not null primary key,
  "DoorId" varchar(255) not null,
  "Status" varchar(255) not null,
  "RequestedAt" timestamp(0) not null,
  "EnteredAt" timestamp(0) not null,
  "LeftAt" timestamp(0) not null
);
