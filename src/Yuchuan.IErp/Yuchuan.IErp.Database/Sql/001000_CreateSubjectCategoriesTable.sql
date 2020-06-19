create table if not exists subject_categories (
  id serial not null primary key,
  name varchar(255) not null,
  book_code varchar(255) not null,
  unique(book_code, name)
);
