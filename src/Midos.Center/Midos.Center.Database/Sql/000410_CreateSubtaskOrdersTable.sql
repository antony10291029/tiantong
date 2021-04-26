create table if not exists "SubtaskOrders" (
  "Id" bigserial not null primary key,
  "SubtypeId" bigint not null,
  "OrderId" bigint not null,
  "SuborderId" bigint not null
);
