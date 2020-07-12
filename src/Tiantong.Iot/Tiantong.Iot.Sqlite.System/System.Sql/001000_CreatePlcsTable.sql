CREATE TABLE IF NOT EXISTS "plcs" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "number" TEXT NOT NULL,
  "model" TEXT NOT NULL,
  "name" TEXT NOT NULL UNIQUE,
  "host" TEXT,
  "port" TEXT,
  "comment" TEXT,
  "created_at" TEXT NOT NULL
);
