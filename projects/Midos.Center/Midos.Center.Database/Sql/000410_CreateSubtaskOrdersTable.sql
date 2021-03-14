create table if not exists subtask_orders (
  "id" bigserial not null,
  "index" int not null,
  "order_id" bigint not null,
  "suborder_id" bigint not null
);
