CREATE TABLE "Locations" (
    "Id" INTEGER IDENTITY(1, 1) PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
    "Latitude" FLOAT DEFAULT 0,
    "Longitude" FLOAT DEFAULT 0,
    "Category" INTEGER DEFAULT 0,
    "Image" TEXT DEFAULT NULL
);