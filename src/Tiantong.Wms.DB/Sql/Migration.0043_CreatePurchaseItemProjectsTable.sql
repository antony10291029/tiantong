create table if not exists purchase_item_projects (
  id serial not null primary key,
  project_id int not null,
  quantity int not null
);
