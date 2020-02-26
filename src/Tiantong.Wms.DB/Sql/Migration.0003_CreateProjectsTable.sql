create table if not exists projects (
  id serial not null primary key,
  number varchar(255) not null,
  repository_ids int[] not null,
  name varchar(255) not null,
  created_at timestamp(0) not null,
  finished_at timestamp(0)
);
