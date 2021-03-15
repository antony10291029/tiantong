create table if not exists subtask_orders (
  "id" bigserial not null,
  "subtype_id" bigint not null,
  "order_id" bigint not null,
  "suborder_id" bigint not null
);
