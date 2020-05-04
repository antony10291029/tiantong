CREATE TABLE IF NOT EXISTS "http_watcher_logs" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "state_id" INTEGER NOT NULL,
  "watcher_id" INTEGER NOT NULL,
  "request" TEXT NOT NULL,
  "response" TEXT NOT NULL,
  "status_code" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
