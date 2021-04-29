create table if not exists "AgcTasks" (
  "Id" bigserial not null primary key,
  "TypeId" bigint not null,
  "Position" varchar(255) not null,
  "Destination" varchar(255) not null,
  "PodCode" varchar(255) not null,
  "AgcCode" varchar(255) not null,
  "Priority" varchar(255) not null,
  "TaskId" varchar(255) not null,
  "RcsTaskCode" varchar(255) not null,
  "Status" varchar(255) not null,
  "CreatedAt" timestamp(0) not null,
  "ClosedAt" timestamp(0) not null
);
