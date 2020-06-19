CREATE TABLE IF NOT EXISTS "http_pusher_logs" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "pusher_id" INTEGER NOT NULL,
  "request" TEXT NOT NULL,
  "response" TEXT NOT NULL,
  "status_code" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
