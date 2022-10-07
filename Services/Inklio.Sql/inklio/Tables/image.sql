CREATE TABLE [inklio].[image]
(
  [id] INT NOT NULL CONSTRAINT [pk_image] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT DEFAULT NULL
    CONSTRAINT [fk_image_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE NO ACTION,
  [content_type] NVARCHAR(128) NULL,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] int NOT NULL
    CONSTRAINT [fk_image_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [edited_at_utc] DATETIME2 NULL,
  [edited_by_id] INT NULL
    CONSTRAINT [fk_image_edited_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [image_class_type_id] TINYINT NOT NULL,
  [delivery_id] INT DEFAULT NULL
    CONSTRAINT [fk_image_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON UPDATE NO ACTION,
  [name] UNIQUEIDENTIFIER NOT NULL,
  [size_in_bytes] BIGINT,
  [thread_id] INT NOT NULL
    CONSTRAINT [fk_image_thread_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE CASCADE,
  [url] NVARCHAR(256) NULL,
)