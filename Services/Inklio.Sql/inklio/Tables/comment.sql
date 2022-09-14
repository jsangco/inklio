CREATE TABLE [inklio].[Comment]
(
  [Id] INT NOT NULL IDENTITY(1,1) CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([Id] ASC),
  [AskId] INT DEFAULT NULL
    CONSTRAINT [FK_Comment_AskId] FOREIGN KEY REFERENCES [Inklio].[Ask] (Id) ON UPDATE NO ACTION,
  [Body] NVARCHAR NOT NULL,
  [CanComment] BIT DEFAULT 1,
  [CanEdit] BIT DEFAULT 1,
  [CanFlag] BIT DEFAULT 1,
  [CreatedAtUtc] DATETIME2 NOT NULL,
  [CreatedById] NVARCHAR(32) NOT NULL,
  [CommentTypeId] TINYINT,
  [DeliveryId] INT DEFAULT NULL
    CONSTRAINT [FK_Comment_DeliveryId] FOREIGN KEY REFERENCES [Inklio].[Delivery] (Id) ON UPDATE NO ACTION,
  [EditedAtUtc] DATETIME2 NULL,
  [EditedById] NVARCHAR(32) NULL,
  [FlagCount] INT DEFAULT 0,
  [IsDeleted] BIT DEFAULT 0,
  [IsLocked] BIT DEFAULT 0,
  [LockedAtUtc] DATETIME2 NULL,
  [SaveCount] INT DEFAULT 0,
  [ThreadId] INT
    CONSTRAINT [FK_Comment_ThreadId] FOREIGN KEY REFERENCES [Inklio].[Ask] (Id) ON UPDATE CASCADE,
  [UpvoteCount] INT DEFAULT 0,
  [ViewCount] int DEFAULT 0,
)
