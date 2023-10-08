CREATE TABLE [inklio].[lock_info]
(
  [id] INT NOT NULL CONSTRAINT [pk_lock_info] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT NOT NULL
    CONSTRAINT [fk_lock_info_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] ([id]) ON UPDATE CASCADE On DELETE CASCADE,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [lock_type] TINYINT NOT NULL,
  [internal_comment] NVARCHAR(max) NOT NULL,
  [user_message] NVARCHAR(max) NOT NULL,
)
