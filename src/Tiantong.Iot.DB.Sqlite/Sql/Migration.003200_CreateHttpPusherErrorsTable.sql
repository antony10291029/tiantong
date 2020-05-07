CREATE TABLE IF NOT EXISTS "http_pusher_errors" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "pusher_id" INTEGER NOT NULL,
  "error" TEXT NOT NULL,
  "detail" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
