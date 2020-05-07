CREATE TABLE IF NOT EXISTS "http_pushers" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "when_opt" TEXT NOT NULL,
  "when_value" TEXT NOT NULL,
  "url" TEXT NOT NULL,
  "value_key" TEXT NOT NULL,
  "data" TEXT NOT NULL,
  "to_string" BOOLEAN NOT NULL
);
