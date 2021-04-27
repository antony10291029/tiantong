create table if not exists "OrderItemReviewRecords" (
  "Id" serial not null primary key,
  "Type" varchar(255) not null,
  "OrderCode" varchar(255) not null,
  "ItemCode" varchar(255) not null,
  "Status" varchar(255) not null,
  "CreatedAt" timestamp(0) not null
);

create index "OrderItemReviewRecords_Type" on "OrderItemReviewRecords" ("Type");
create index "OrderItemReviewRecords_OrderCode" on "OrderItemReviewRecords" ("OrderCode");
create index "OrderItemReviewRecords_ItemCode" on "OrderItemReviewRecords" ("ItemCode");
create index "OrderItemReviewRecords_Status" on "OrderItemReviewRecords" ("Status");
