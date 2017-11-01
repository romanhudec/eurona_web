
CREATE TABLE tEmailLog
(
	Id [int] IDENTITY(1,1) NOT NULL,
	Email NVARCHAR(100) NOT NULL,
	[Subject] NVARCHAR(500) NULL,
	[Message] NVARCHAR(MAX) NULL,
	[Status] BIT NOT NULL,
	[Error] NVARCHAR(MAX) NULL,
	Timestamp DATETIME NOT NULL,
 CONSTRAINT [PK_EmailLog] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]


-- !!!! NAKOPIROVAT !!!!!
-- d:\sk\Eurona\eurona\eurona\images\Kategorie\.. 
