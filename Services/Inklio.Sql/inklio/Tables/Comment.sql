CREATE TABLE [dbo].[Comment]
(
  [id] INT NOT NULL PRIMARY KEY,
  [askId] INT FOREIGN KEY REFERENCES Ask(id) DEFAULT NULL,
  [deliveryId] INT FOREIGN KEY REFERENCES Delivery(id) DEFAULT NULL,
  [body] NVARCHAR NOT NULL,
  [canComment] BIT DEFAULT 1,
  [canEdit] BIT DEFAULT 1,
  [canFlag] BIT DEFAULT 1,
  [createdAtUtc] DATETIME2 NOT NULL,
  [createdById] NVARCHAR(32) NOT NULL,
  [editedAtUtc] DATETIME2 NULL,
  [editedById] NVARCHAR(32) NULL,
  [flagCount] INT DEFAULT 0,
  [isDeleted] BIT DEFAULT 0,
  [isLocked] BIT DEFAULT 0,
  [lockedAtUtc] DATETIME2 NULL,
  [saveCount] INT DEFAULT 0,
  [upvoteCount] INT DEFAULT 0,
  [views] int DEFAULT 0,
)
