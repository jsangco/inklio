CREATE TABLE [inklio].[challenge]
(
  [id] INT NOT NULL CONSTRAINT [pk_challenge] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT NOT NULL
    CONSTRAINT [fk_challenge_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] ([id]) ON UPDATE CASCADE,
  [challenge_type_id] TINYINT NOT NULL,
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [end_at_utc] DATETIME2 NOT NULL,
  [start_at_utc] DATETIME2 NOT NULL,
  [challenge_state_id] TINYINT NOT NULL,
)
