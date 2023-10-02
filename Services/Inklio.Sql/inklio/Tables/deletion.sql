CREATE TABLE [inklio].[deletion]
(
  [id] INT NOT NULL CONSTRAINT [pk_deletion] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT DEFAULT NULL
    CONSTRAINT [fk_deletion_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE NO ACTION,
  [comment_id] INT DEFAULT NULL
    CONSTRAINT [fk_deletion_comment_id] FOREIGN KEY REFERENCES [inklio].[comment] (id) ON UPDATE NO ACTION,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] int NOT NULL
    CONSTRAINT [fk_deletion_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [deletion_class_type_id] TINYINT NOT NULL,
  [deletion_type_id] int NOT NULL,
  [delivery_id] INT DEFAULT NULL
    CONSTRAINT [fk_deletion_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON UPDATE NO ACTION,
  [internal_comment] NVARCHAR(max) NOT NULL,
  [user_message] NVARCHAR(max) NOT NULL,
  CONSTRAINT uc_deletion UNIQUE (created_by_id, ask_id, comment_id, delivery_id),
)
