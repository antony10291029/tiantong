CREATE TABLE IF NOT EXISTS "plc_state_errors" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "state_id" INTEGER NOT NULL,
  "operation" TEXT NOT NULL,
  "value" TEXT,
  "message" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
