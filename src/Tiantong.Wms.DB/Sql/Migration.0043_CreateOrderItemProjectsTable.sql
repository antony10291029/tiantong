create table if not exists order_item_projects (
  id serial not null primary key,
  order_item_id int not null,
  project_id int not null,
  "index" int not null,
  quantity int not null
);
