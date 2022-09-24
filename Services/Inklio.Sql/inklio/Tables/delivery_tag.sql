CREATE TABLE [inklio].[delivery_tag]
(
  [delivery_id] INT NOT NULL,
  [tag_id] INT NOT NULL,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  CONSTRAINT pk_delivery_tag PRIMARY KEY ([delivery_id], [tag_id]),
  CONSTRAINT [fk_delivery_tag_delivery_id] FOREIGN KEY ([delivery_id]) REFERENCES [inklio].[delivery] ([id]) ON DELETE CASCADE,
  CONSTRAINT [fk_delivery_tag_tag_id] FOREIGN KEY ([tag_id]) REFERENCES [inklio].[tag] ([id]) ON DELETE CASCADE,
  CONSTRAINT [fk_delivery_tag_user_id] FOREIGN KEY ([created_by_id]) REFERENCES [inklio].[user] ([id]) ON DELETE CASCADE,
)
