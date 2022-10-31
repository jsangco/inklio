CREATE TABLE [dbo].[OpenIddictScopes](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyToken] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Descriptions] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[DisplayNames] [nvarchar](max) NULL,
	[Name] [nvarchar](200) NULL,
	[Properties] [nvarchar](max) NULL,
	[Resources] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[OpenIddictScopes] ADD  CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OpenIddictScopes_Name] ON [dbo].[OpenIddictScopes]
(
	[Name] ASC
)
WHERE ([Name] IS NOT NULL)
WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
