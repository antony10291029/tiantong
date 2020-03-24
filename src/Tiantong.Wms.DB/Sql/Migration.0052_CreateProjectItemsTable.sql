create table if not exists project_items (
  id serial not null primary key,
  warehouse_id int not null,
  project_id int not null,
  order_id int not null,
  item_id int not null,
  quantity int not null,
  unique(project_id, order_id, item_id)
);
