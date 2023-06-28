CREATE TABLE [inklio].[user]
(
  [id] INT NOT NULL CONSTRAINT [pk_user] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [created_at_utc] DATETIME2 NOT NULL,
  [delivery_count] INT NOT NULL DEFAULT 0,
  [delivery_reputation] INT NOT NULL DEFAULT 0,
  [last_activity_at_utc] DATETIME2 NOT NULL,
  [last_login_at_utc] DATETIME2 NOT NULL,
  [reputation] INT NOT NULL DEFAULT 0,
  [username] NVARCHAR(32) NOT NULL,
  CONSTRAINT [ak_username] UNIQUE(username),
)
