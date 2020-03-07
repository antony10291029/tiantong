create table if not exists order_projects (
  "key" int not null,
  order_id int not null,
  project_id int not null,
  order_item_ids int[] not null,
  primary key("key", order_id, project_id)
);
