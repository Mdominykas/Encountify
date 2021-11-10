CREATE TABLE "Users" (
    "Id" INTEGER PRIMARY KEY,
    "Username" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "IsAdmin" BIT DEFAULT 0,
    "Image" TEXT
);