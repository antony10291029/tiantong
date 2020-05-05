CREATE TABLE IF NOT EXISTS "http_watcher_errors" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "state_id" INTEGER NOT NULL,
  "watcher_id" INTEGER NOT NULL,
  "error" TEXT NOT NULL,
  "detail" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
