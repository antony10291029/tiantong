CREATE TABLE IF NOT EXISTS "http_pushers" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "name" TEXT NOT NULL,
  "number" TEXT NOT NULL,
  "url" TEXT NOT NULL,
  "body" TEXT NOT NULL,
  "header" TEXT NOT NULL,
  "when_opt" TEXT NOT NULL,
  "when_value" TEXT NOT NULL,
  "value_key" TEXT NOT NULL,
  "is_value_to_string" BOOLEAN NOT NULL,
  "is_concurrent" BOOLEAN NOT NULL
);
