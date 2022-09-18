CREATE TABLE [inklio].[ask_tag]
(
  [id] INT NOT NULL CONSTRAINT [PK_tag] PRIMARY KEY CLUSTERED ([id] ASC),
  [ask_id] INT DEFAULT NULL
    CONSTRAINT [FK_ask_tag_ask_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON DELETE CASCADE,
  [tag_id] INT DEFAULT NULL
    CONSTRAINT [FK_ask_tag_tag_tag_id] FOREIGN KEY REFERENCES [inklio].[tag] (id) ON DELETE CASCADE,
  [delivery_id] INT DEFAULT NULL
    CONSTRAINT [FK_tag_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON UPDATE NO ACTION,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
)
