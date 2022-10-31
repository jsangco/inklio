CREATE TABLE [dbo].[OpenIddictTokens](
	[Id] [nvarchar](450) NOT NULL,
	[ApplicationId] [nvarchar](450) NULL,
	[AuthorizationId] [nvarchar](450) NULL,
	[ConcurrencyToken] [nvarchar](50) NULL,
	[CreationDate] [datetime2](7) NULL,
	[ExpirationDate] [datetime2](7) NULL,
	[Payload] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
	[RedemptionDate] [datetime2](7) NULL,
	[ReferenceId] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	[Subject] [nvarchar](400) NULL,
	[Type] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[OpenIddictTokens] ADD  CONSTRAINT [PK_OpenIddictTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OpenIddictTokens_ApplicationId_Status_Subject_Type] ON [dbo].[OpenIddictTokens]
(
	[ApplicationId] ASC,
	[Status] ASC,
	[Subject] ASC,
	[Type] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OpenIddictTokens_AuthorizationId] ON [dbo].[OpenIddictTokens]
(
	[AuthorizationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OpenIddictTokens_ReferenceId] ON [dbo].[OpenIddictTokens]
(
	[ReferenceId] ASC
)
WHERE ([ReferenceId] IS NOT NULL)
WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OpenIddictTokens] ADD CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[OpenIddictApplications] ([Id])
GO
ALTER TABLE [dbo].[OpenIddictTokens] CHECK CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId]
GO
ALTER TABLE [dbo].[OpenIddictTokens] ADD  CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY([AuthorizationId])
REFERENCES [dbo].[OpenIddictAuthorizations] ([Id])
GO
ALTER TABLE [dbo].[OpenIddictTokens] CHECK CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId]
GO
