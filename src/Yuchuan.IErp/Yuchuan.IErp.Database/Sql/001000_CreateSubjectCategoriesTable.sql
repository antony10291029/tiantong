create table if not exists subject_categories (
  id serial not null primary key,
  name varchar(255) not null,
  book_code varchar(255) not null,
  unique(book_code, name)
);

create table if not exists subject_sub_categories (
  id serial not null primary key,
  category_id integer not null,
  name varchar(255) not null,
  subject_code varchar(255) not null,
  unique(category_id, name)
);
