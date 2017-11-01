
CREATE TABLE tBonusovyKreditLog
(
	Id INT IDENTITY(1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[BonusovyKreditTyp] INT NULL,
	[Message] NVARCHAR(MAX) NULL,
	[Status] NVARCHAR(100) NULL,
	Timestamp DATETIME NOT NULL,
 CONSTRAINT [PK_BonusovyKreditLog] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]


INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (1, 'ESHOP_PLATBAKARTOU_LIMIT', 'ESHOP', 'Limit pro platbu kartou', '15')
GO