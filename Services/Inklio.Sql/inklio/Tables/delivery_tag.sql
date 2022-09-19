CREATE TABLE [inklio].[delivery_tag]
(
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [delivery_id] INT
    CONSTRAINT [fk_tag_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON DELETE CASCADE,
  [tag_id] INT
    CONSTRAINT [fk_delivery_tag_tag_tag_id] FOREIGN KEY REFERENCES [inklio].[tag] (id) ON DELETE CASCADE,
  CONSTRAINT pk_delivery_tag PRIMARY KEY (delivery_id, tag_id)
)
