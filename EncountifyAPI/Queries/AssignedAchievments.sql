CREATE TABLE "AssignedAchievments" (
    "Id" INTEGER IDENTITY(1, 1) PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "AchievmentId" INTEGER NOT NULL,
    "AssignmentDate" TEXT
);