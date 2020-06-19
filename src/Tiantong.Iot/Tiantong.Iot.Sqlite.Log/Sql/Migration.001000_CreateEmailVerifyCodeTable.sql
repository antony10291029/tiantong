CREATE TABLE IF NOT EXISTS "email_verify_code" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "email" TEXT NOT NULL,
  "verify_code" TEXT NOT NULL,
  "is_verified" BOOLEAN NOT NULL,
  "expired_at" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
