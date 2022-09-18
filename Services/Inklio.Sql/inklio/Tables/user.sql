CREATE TABLE [inklio].[user]
(
  [id] INT NOT NULL CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED ([id] ASC),
  [created_at_utc] DATETIME2 NOT NULL,
  [delivery_reputation] INT NOT NULL DEFAULT 0,
  [last_access_at_utc] DATETIME2 NOT NULL,
  [reputation] INT NOT NULL DEFAULT 0,
  [username] NVARCHAR(32) NOT NULL,
)
