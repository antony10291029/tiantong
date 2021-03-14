create table if not exists task_orders (
  "id" bigserial not null primary key,
  "type_id" bigint not null,
  "status" varchar(255) not null,
  "data" text not null,
  "created_at" timestamp(0) not null,
  "started_at" timestamp(0) not null,
  "closed_at" timestamp(0) not null
);
