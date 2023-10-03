-- This is a helper script for dropping every table when re-initializing the DB. It is wrapped
-- in a transaction with a rollback in the event of any unintentional execution.

BEGIN TRANSACTION

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetRoleClaims]') AND type in (N'U'))
DROP TABLE [auth].[AspNetRoleClaims]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetUserClaims]') AND type in (N'U'))
DROP TABLE [auth].[AspNetUserClaims]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetUserLogins]') AND type in (N'U'))
DROP TABLE [auth].[AspNetUserLogins]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetUserRoles]') AND type in (N'U'))
DROP TABLE [auth].[AspNetUserRoles]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetUserTokens]') AND type in (N'U'))
DROP TABLE [auth].[AspNetUserTokens]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetRoles]') AND type in (N'U'))
DROP TABLE [auth].[AspNetRoles]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[AspNetUsers]') AND type in (N'U'))
DROP TABLE [auth].[AspNetUsers]


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[ask_tag]') AND type in (N'U'))
DROP TABLE [inklio].[ask_tag]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[delivery_tag]') AND type in (N'U'))
DROP TABLE [inklio].[delivery_tag]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[flag]') AND type in (N'U'))
DROP TABLE [inklio].[flag]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[image]') AND type in (N'U'))
DROP TABLE [inklio].[image]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[tag]') AND type in (N'U'))
DROP TABLE [inklio].[tag]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[upvote]') AND type in (N'U'))
DROP TABLE [inklio].[upvote]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[comment]') AND type in (N'U'))
DROP TABLE [inklio].[comment]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[delivery]') AND type in (N'U'))
DROP TABLE [inklio].[delivery]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[ask]') AND type in (N'U'))
DROP TABLE [inklio].[ask]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[inklio].[user]') AND type in (N'U'))
DROP TABLE [inklio].[user]

ROLLBACK;