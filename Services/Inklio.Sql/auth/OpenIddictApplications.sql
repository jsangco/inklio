CREATE TABLE [dbo].[OpenIddictApplications](
	[Id] [nvarchar](450) NOT NULL,
	[ClientId] [nvarchar](100) NULL,
	[ClientSecret] [nvarchar](max) NULL,
	[ConcurrencyToken] [nvarchar](50) NULL,
	[ConsentType] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[DisplayNames] [nvarchar](max) NULL,
	[Permissions] [nvarchar](max) NULL,
	[PostLogoutRedirectUris] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
	[RedirectUris] [nvarchar](max) NULL,
	[Requirements] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[OpenIddictApplications] ADD  CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OpenIddictApplications_ClientId] ON [dbo].[OpenIddictApplications]
(
	[ClientId] ASC
)
WHERE ([ClientId] IS NOT NULL)
WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
