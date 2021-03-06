
CREATE TABLE tShpOrderSettings
(
	Id INT IDENTITY(1,1) NOT NULL,
	InstanceId INT NOT NULL,
	[Code] NVARCHAR(100) NOT NULL,
	[Value] DECIMAL(19,6) NULL,
	[Locale] NVARCHAR(2) NOT NULL,
	[Enabled] BIT NOT NULL,
 CONSTRAINT [PK_tShpOrderSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]


INSERT INTO tShpOrderSettings (InstanceId, Code, Value, Locale, [Enabled] ) VALUES (1, 'ESHOP_ORDER_FREEE_POSTAGE_CS', 1500, 'cs', 1)
INSERT INTO tShpOrderSettings (InstanceId, Code, Value, Locale, [Enabled] ) VALUES (1, 'ESHOP_ORDER_FREEE_POSTAGE_SK', 63, 'sk', 1)
INSERT INTO tShpOrderSettings (InstanceId, Code, Value, Locale, [Enabled] ) VALUES (1, 'ESHOP_ORDER_FREEE_POSTAGE_PL', 350, 'pl', 1)
GO