CREATE TABLE [inklio].[ask]
(
  [id] INT NOT NULL CONSTRAINT [pk_ask] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [body] NVARCHAR(max) NOT NULL,
  [can_comment] BIT NOT NULL DEFAULT 1,
  [can_deliver] BIT NOT NULL DEFAULT 1,
  [can_edit] BIT NOT NULL DEFAULT 1,
  [can_flag] BIT NOT NULL DEFAULT 1,
  [can_tag] BIT NOT NULL DEFAULT 1,
  [comment_count] INT NOT NULL DEFAULT 0,
  [content_rating] TINYINT NOT NULL DEFAULT 0,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL
    CONSTRAINT [fk_ask_created_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [delivery_count] INT NOT NULL DEFAULT 0,
  [delivery_accepted_count] INT NOT NULL DEFAULT 0,
  [edited_at_utc] DATETIME2 NULL,
  [edited_by_id] INT NULL
    CONSTRAINT [fk_ask_edited_by_id_user_id] FOREIGN KEY REFERENCES [inklio].[user] ([id]) ON UPDATE NO ACTION,
  [flag_count] INT NOT NULL DEFAULT 0,
  [is_deleted] BIT NOT NULL DEFAULT 0,
  [is_delivered] BIT NOT NULL DEFAULT 0,
  [is_delivery_accepted] BIT NOT NULL DEFAULT 0,
  [is_locked] BIT NOT NULL DEFAULT 0,
  [is_spoiler] BIT NOT NULL DEFAULT 0,
  [locked_at_utc] DATETIME2 NULL,
  [rank_hot] int NOT NULL DEFAULT 0,
    INDEX ix_rank_hot NONCLUSTERED (rank_hot),
  [save_count] INT NOT NULL DEFAULT 0,
  [title] NVARCHAR(256) NOT NULL,
  [upvote_count] INT NOT NULL DEFAULT 0,
  [view_count] int NOT NULL DEFAULT 0,
)
