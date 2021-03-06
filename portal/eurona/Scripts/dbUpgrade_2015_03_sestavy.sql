
--<add key="SHP:DocumentGallery:Product:StoragePath" value="~/userfiles/eshop/documents/product/"/>
CREATE TABLE tShpDokumentProduktuEmail
(
	[Email] NVARCHAR(100) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Info] NVARCHAR (1000) NULL,
	[Timestamp] DATETIME NOT NULL
)
GO

DECLARE @InstanceId INT
SET @InstanceId = 1
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/datovyList.aspx', @Locale='cs', @Alias = '~/datovy-list', @Name='Datovy List'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/datovyList.aspx', @Locale='sk', @Alias = '~/datovy-list', @Name='Datovy List'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/datovyList.aspx', @Locale='en', @Alias = '~/datovy-list', @Name='Datovy List'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/datovyList.aspx', @Locale='pl', @Alias = '~/datovy-list', @Name='Datovy List'
