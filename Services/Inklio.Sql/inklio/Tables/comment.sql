CREATE TABLE [inklio].[comment]
(
  [id] INT NOT NULL CONSTRAINT [PK_comment] PRIMARY KEY CLUSTERED ([id] ASC),
  [ask_id] INT DEFAULT NULL
    CONSTRAINT [FK_comment_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE NO ACTION,
  [body] NVARCHAR(max) NOT NULL,
  [can_comment] BIT NOT NULL DEFAULT 1,
  [can_edit] BIT NOT NULL DEFAULT 1,
  [can_flag] BIT NOT NULL DEFAULT 1,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] int NOT NULL,
    CONSTRAINT [FK_comment_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE CASCADE,
  [comment_class_type_id] TINYINT NOT NULL,
  [delivery_id] INT DEFAULT NULL
    CONSTRAINT [FK_comment_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] (id) ON UPDATE NO ACTION,
  [edited_at_utc] DATETIME2 NULL,
  [edited_by_id] INT NULL,
    CONSTRAINT [FK_comment_edited_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE CASCADE,
  [flag_count] INT NOT NULL DEFAULT 0,
  [is_deleted] BIT NOT NULL DEFAULT 0,
  [is_locked] BIT NOT NULL DEFAULT 0,
  [locked_at_utc] DATETIME2 NULL,
  [save_count] INT NOT NULL DEFAULT 0,
  [thread_id] INT NOT NULL
    CONSTRAINT [FK_comment_thread_id] FOREIGN KEY REFERENCES [inklio].[ask] (id) ON UPDATE CASCADE,
  [upvote_count] INT NOT NULL DEFAULT 0,
  [view_count] int NOT NULL DEFAULT 0,
)
