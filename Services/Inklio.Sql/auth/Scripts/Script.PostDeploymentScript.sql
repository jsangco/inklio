-- This file contains SQL statements that will be executed after the build script.

-- Fill database with default roles
IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[AspNetRoles] WHERE [NormalizedName] = 'USER')
    INSERT INTO [auth].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) Values('57cef82c-22f8-405d-8e7d-77ce5d20e623', 'User', 'USER', 'b6910140-8936-4c4d-929f-8ea27991dfd1')

IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[AspNetRoles] WHERE [NormalizedName] = 'ADMINISTRATOR')
    INSERT INTO [auth].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) Values('73daa349-7ed4-41d4-a148-e643c0e4c909', 'Administrator', 'ADMINISTRATOR', '4c098da1-ed55-4f6e-88ea-8a1dc7731979')

IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[AspNetRoles] WHERE [NormalizedName] = 'MODERATOR')
    INSERT INTO [auth].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) Values('c8b0b8a8-48f0-4647-a84a-2404b8a899e0', 'Moderator', 'MODERATOR', 'dd2845af-40b0-41c4-b001-5e9a85aaec09')