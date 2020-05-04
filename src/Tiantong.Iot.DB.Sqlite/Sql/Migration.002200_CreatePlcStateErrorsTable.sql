CREATE TABLE IF NOT EXISTS "plc_state_errors" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "state_id" INTEGER NOT NULL,
  "error" TEXT NOT NULL,
  "detail" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
