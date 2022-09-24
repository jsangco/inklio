CREATE TABLE [inklio].[flag]
(
  [id] INT NOT NULL CONSTRAINT [pk_flag] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT DEFAULT NULL
    CONSTRAINT [fk_flag_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE NO ACTION,
  [comment_id] INT DEFAULT NULL
    CONSTRAINT [fk_flag_comment_id] FOREIGN KEY REFERENCES [inklio].[comment] (id) ON UPDATE NO ACTION,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] int NOT NULL
    CONSTRAINT [fk_flag_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [delivery_id] INT DEFAULT NULL
    CONSTRAINT [fk_flag_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON UPDATE NO ACTION,
  [type_id] int NOT NULL,
  [flag_class_type_id] TINYINT NOT NULL,
  CONSTRAINT uc_flag UNIQUE (created_by_id, ask_id, comment_id, delivery_id),
)
