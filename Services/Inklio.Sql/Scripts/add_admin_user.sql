-- This script is used automatically adding a admin user named 'aoeu' with password 'aoeuaoeu1'.
-- This script should only be used on the local SQL server instance and not in production.

-- Create admin account
IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[AspNetUsers] WHERE [NormalizedUserName] = 'AOEU')
  INSERT INTO [auth].[AspNetUsers] (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
  VALUES ('805ae550-5e8b-41e7-b7ba-08dbc3a6f1de','aoeu','AOEU','aoeuaoeuaoeu@mailinator.com','AOEUAOEUAOEU@MAILINATOR.COM',	0,	'AQAAAAIAAYagAAAAEAZ5M3kFW4Qr9IIPc8SZMOGCajfmPBhQODOvXrTiCEv7G3/gO7X8gh/08UVgtcSV/g==', 'INKJSSJP73J6CHYLJ4KJN6O5QLYKLTKW','2e326bae-d16f-4e72-96ea-5ee4d77e7f51',NULL,0,0,NULL,1,0)

-- Set account as admin
DECLARE @adminUserId UNIQUEIDENTIFIER
DECLARE @adminRoleId UNIQUEIDENTIFIER
SELECT @adminUserId = [Id] from [auth].[AspNetUsers] where [NormalizedUserName] = 'AOEU'
SELECT @adminRoleId = [Id] from [auth].[AspNetRoles] where [NormalizedName] = 'ADMINISTRATOR'
IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[AspNetUserRoles] WHERE [UserId] = @adminUserId AND [RoleId] <> @adminRoleId)
  INSERT INTO [auth].[AspNetUserRoles] (UserId, RoleId)
  VALUES (@adminUserId, @adminRoleId)

-- Create inklio user for the admin account
IF NOT EXISTS (SELECT TOP 1 1 FROM [inklio].[user] WHERE [user_id] = @adminUserId)
  INSERT INTO [inklio].[user] (user_id, username, created_at_utc, last_activity_at_utc, last_login_at_utc)
  VALUES (@adminUserId, 'aoeu', GETUTCDATE(), GETUTCDATE(), GETUTCDATE())
