CREATE TABLE [inklio].[tag]
(
  [id] INT NOT NULL CONSTRAINT [pk_tag] PRIMARY KEY CLUSTERED IDENTITY(1,1),
  [created_at_utc] DATETIME2 NOT NULL,
  [created_by_id] INT NOT NULL,
  [type] NVARCHAR(64) NOT NULL,
  [value] NVARCHAR(64) NOT NULL,
  CONSTRAINT [ak_tag_type_value] UNIQUE ([type], [value])
)
