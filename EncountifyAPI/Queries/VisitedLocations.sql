CREATE TABLE "VisitedLocations" (
    "Id" INTEGER IDENTITY(1, 1) PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "LocationId" INTEGER NOT NULL,
    "Points" INTEGER DEFAULT 0
);