CREATE TABLE IF NOT EXISTS "http_watchers" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "state_id" INTEGER NOT NULL,
  "opt" TEXT NOT NULL,
  "value" TEXT NOT NULL,
  "url" TEXT NOT NULL,
  "value_key" TEXT NOT NULL,
  "data" TEXT NOT NULL,
  "to_string" BOOLEAN NOT NULL
);
