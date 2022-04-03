CREATE TABLE "Achievments" (
    "Id" INTEGER IDENTITY(1, 1) PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
    "Category" INTEGER DEFAULT 0,
    "Image" TEXT DEFAULT NULL
);