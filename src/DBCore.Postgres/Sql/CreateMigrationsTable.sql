create table if not exists "_migrations" (
  "id" serial not null primary key,
  "batch_id" integer not null,
  "file_name" varchar(255) not null,
  "created_at" timestamp not null
);
