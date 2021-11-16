CREATE TABLE "Users" (
    "Id" INTEGER IDENTITY(1, 1) PRIMARY KEY,
    "Username" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "IsAdmin" BIT DEFAULT 0,
    "Picture" TEXT DEFAULT NULL,
    "DateCreated" DATE NOT NULL,
    "Points" INTEGER DEFAULT 0
);