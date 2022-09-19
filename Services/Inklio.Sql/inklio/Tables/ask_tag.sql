CREATE TABLE [inklio].[ask_tag]
(
  [ask_id] INT
    CONSTRAINT [fk_ask_tag_ask_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON DELETE CASCADE,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [tag_id] INT
    CONSTRAINT [fk_ask_tag_tag_tag_id] FOREIGN KEY REFERENCES [inklio].[tag] (id) ON DELETE CASCADE,
  CONSTRAINT pk_ask_tag PRIMARY KEY (ask_id, tag_id)
)
