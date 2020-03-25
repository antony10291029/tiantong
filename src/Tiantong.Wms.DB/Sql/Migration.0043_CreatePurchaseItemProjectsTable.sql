create table if not exists project_items (
  id serial not null primary key,
  project_id int not null,
  quantity int not null
);
