USE [TVD]
GO

CREATE TABLE [dbo].[www_odberatele_strom](
	[Id] [int] NOT NULL,
	[Id_Odberatele] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LineageId] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_www_odberatele_strom] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]

GO


