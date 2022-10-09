CREATE TABLE [inklio].[delivery]
(
  [id] INT NOT NULL CONSTRAINT [pk_delivery] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [accepted_at_utc] DATETIME2 NULL,
  [accepted_undo_at_utc] DATETIME2 NULL,
  [ask_id] INT NOT NULL
    CONSTRAINT [fk_delivery_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] ([id]) ON UPDATE CASCADE,
  [body] NVARCHAR(max) NOT NULL,
  [can_comment] BIT NOT NULL DEFAULT 1,
  [can_edit] BIT NOT NULL DEFAULT 1,
  [can_flag] BIT NOT NULL DEFAULT 1,
  [can_tag] BIT NOT NULL DEFAULT 1,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL
    CONSTRAINT [fk_delivery_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [edited_at_utc] DATETIME2 NULL,
  [edited_by_id] INT NULL
    CONSTRAINT [fk_delivery_edited_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [flag_count] INT NOT NULL DEFAULT 0,
  [is_deleted] BIT NOT NULL DEFAULT 0,
  [is_delivery_accepted] BIT DEFAULT 0,
  [is_locked] BIT NOT NULL DEFAULT 0,
  [is_nsfw] BIT NOT NULL DEFAULT 0,
  [is_nsfl] BIT NOT NULL DEFAULT 0,
  [locked_at_utc] DATETIME2 NULL,
  [save_count] INT NOT NULL DEFAULT 0,
  [title] NVARCHAR(256) NOT NULL,
  [upvote_count] INT NOT NULL DEFAULT 0,
  [view_count] int NOT NULL DEFAULT 0,
)
