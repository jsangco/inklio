CREATE TABLE [inklio].[user]
(
  [id] INT NOT NULL CONSTRAINT [pk_user] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_count] INT NOT NULL DEFAULT 0,
  [ask_reputation] INT NOT NULL DEFAULT 0,
  [delivery_count] INT NOT NULL DEFAULT 0,
  [delivery_reputation] INT NOT NULL DEFAULT 0,
  [comment_count] INT NOT NULL DEFAULT 0,
  [comment_reputation] INT NOT NULL DEFAULT 0,
  [created_at_utc] DATETIME2 NOT NULL,
  [last_activity_at_utc] DATETIME2 NOT NULL,
  [last_login_at_utc] DATETIME2 NOT NULL,
  [reputation] INT NOT NULL DEFAULT 0,
  [user_id] UNIQUEIDENTIFIER NOT NULL,
  [username] NVARCHAR(32) NOT NULL,
  CONSTRAINT [ak_username] UNIQUE(username),
  CONSTRAINT [ak_user_id] UNIQUE(user_id),
)
