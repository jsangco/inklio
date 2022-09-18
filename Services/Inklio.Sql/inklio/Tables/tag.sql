CREATE TABLE [inklio].[tag]
(
  [id] INT NOT NULL CONSTRAINT [PK_tag] PRIMARY KEY CLUSTERED ([id] ASC),
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [type] NVARCHAR(256) NOT NULL,
  [value] NVARCHAR(256) NOT NULL,
)
