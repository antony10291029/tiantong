create table if not exists order_projects (
  id serial not null primary key,
  "key" int not null,
  order_id int not null,
  project_id int not null,
  order_item_ids int[] not null,
  unique(order_id, project_id, "key")
);
