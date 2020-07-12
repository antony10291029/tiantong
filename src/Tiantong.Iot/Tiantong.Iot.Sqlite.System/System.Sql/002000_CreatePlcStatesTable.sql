CREATE TABLE IF NOT EXISTS "plc_states" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "name" TEXT NOT NULL,
  "number" TEXT NOT NULL,
  "type" TEXT NOT NULL,
  "address" TEXT NOT NULL,
  "length" INTEGER NOT NULL,
  "is_heartbeat" BOOLEAN NOT NULL,
  "heartbeat_interval" INTEGER NOT NULL,
  "heartbeat_max_value" INTEGER NOT NULL,
  "is_collect" BOOLEAN NOT NULL,
  "collect_interval" INTEGER NOT NULL,
  "is_read_log_on" BOOLEAN NOT NULL,
  "is_write_log_on" BOOLEAN NOT NULL,
  UNIQUE("plc_id", "name")
);
