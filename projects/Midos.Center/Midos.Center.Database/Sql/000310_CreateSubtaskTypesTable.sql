create table if not exists subtask_types (
  "id" bigserial not null,
  "index" int not null,
  "type_id" bigint not null,
  "subtype_id" bigint not null
);
