create table if not exists "_migrations" (
  "id" serial not null primary key,
  "batch_id" integer not null,
  "file_name" TEXT not null,
  "created_at" TEXT not null
);
