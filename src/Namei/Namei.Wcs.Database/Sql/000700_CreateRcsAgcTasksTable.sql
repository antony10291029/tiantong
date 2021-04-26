create table if not exists "RcsAgcTasks" (
  "Id" bigserial not null primary key,
  "Position" varchar(255) not null,
  "Destination" varchar(255) not null,
  "Priority" varchar(255) not null,
  "Comment" text not null,
  "TaskType" varchar(255) not null,
  "TaskCode" varchar(255) not null,
  "OrderType" varchar(255) not null,
  "OrderId" bigint not null,
  "PodCode" varchar(255) not null,
  "AgcCode" varchar(255) not null,
  "Status" varchar(255) not null,
  "CreatedAt" timestamp(0) not null,
  "StartedAt" timestamp(0) not null,
  "ClosedAt" timestamp(0) not null
);
