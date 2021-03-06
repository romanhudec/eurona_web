------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
-- AccountExt
CREATE TABLE [tAccountExt](
	[AccountId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[AdvisorId] [int] NULL
)
GO

ALTER TABLE [tAccountExt]  WITH CHECK 
	ADD CONSTRAINT [FK_tAccountExt_AdvisorId] FOREIGN KEY([AdvisorId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountExt] CHECK CONSTRAINT [FK_tAccountExt_AdvisorId]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tLoggedAccount](
	[AccountId] [int] NOT NULL,
	[LoggedAt] [DATETIME] NOT NULL,
	[InstanceId] [int] NOT NULL
)
GO

CREATE TABLE [tError]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Stamp] [DATETIME] NOT NULL,
	[Location] NVARCHAR(500) NULL,
	[InstanceId] [int] NOT NULL,
	[Exception] NVARCHAR(MAX) NULL,
	[StackTrace] NVARCHAR(MAX) NULL
)
GO

------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tMimoradnaNabidka](
	[MimoradnaNabidkaId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tMimoradnaNabidka_Locale]  DEFAULT ('en'),
	[Date] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[UrlAliasId] [int] NULL,
 CONSTRAINT [PK_MimoradnaNabidkaId] PRIMARY KEY CLUSTERED ([MimoradnaNabidkaId] ASC)
)
GO

ALTER TABLE [tMimoradnaNabidka]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMimoradnaNabidka_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tMimoradnaNabidka] CHECK CONSTRAINT [FK_tMimoradnaNabidka_UrlAliasId]
GO


------------------------------------------------------------------------------------------------------------------------
-- tAngelTeamSettings
CREATE TABLE [tAngelTeamSettings](
	[DisableATP] [BIT] NOT NULL DEFAULT(0), 
	[MaxViewPerMinute] [int] NOT NULL DEFAULT(0),
	[BlockATPHours] [int] NOT NULL DEFAULT(0),
)
GO
------------------------------------------------------------------------------------------------------------------------
-- tAngelTeamViews
CREATE TABLE [tAngelTeamViews](
	[AccountId] [int] NOT NULL,
	[ViewDate] [DATETIME] NOT NULL,
	[ViewCount] [int] NOT NULL DEFAULT(0),
)
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------
