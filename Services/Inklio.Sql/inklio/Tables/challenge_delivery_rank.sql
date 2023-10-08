CREATE TABLE [inklio].[challenge_delivery_rank]
(
  [id] INT NOT NULL CONSTRAINT [pk_challenge_delivery_rank] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [ask_id] INT NOT NULL
    CONSTRAINT [fk_challenge_delivery_rank_ask_id] FOREIGN KEY REFERENCES [inklio].[ask] ([id]),
  [challenge_id] INT NOT NULL
    CONSTRAINT [fk_challenge_delivery_rank_challenge_id] FOREIGN KEY REFERENCES [inklio].[challenge] ([id]) ON UPDATE CASCADE,
  [delivery_id] INT NOT NULL
    CONSTRAINT [fk_challenge_delivery_rank_delivery_id] FOREIGN KEY REFERENCES [inklio].[delivery] ([id]),
  [created_at_utc] DATETIME2 NOT NULL,
  [rank] INT NOT NULL,
)
