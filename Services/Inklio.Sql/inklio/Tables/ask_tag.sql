CREATE TABLE [inklio].[ask_tag]
(
  [ask_id] INT NOT NULL,
  [tag_id] INT NOT NULL,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  CONSTRAINT pk_ask_tag PRIMARY KEY ([ask_id], [tag_id]),
  CONSTRAINT [fk_ask_tag_ask_id] FOREIGN KEY ([ask_id]) REFERENCES [inklio].[ask] ([id]) ON DELETE CASCADE,
  CONSTRAINT [fk_ask_tag_tag_id] FOREIGN KEY ([tag_id]) REFERENCES [inklio].[tag] ([id]) ON DELETE CASCADE,
  CONSTRAINT [fk_ask_tag_user_id] FOREIGN KEY ([created_by_id]) REFERENCES [inklio].[user] ([id]) ON DELETE CASCADE,
)
