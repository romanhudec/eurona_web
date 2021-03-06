CREATE TABLE [dbo].[tShpLastOrderAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[FirstName] [nvarchar](200) NULL,
	[LastName] [nvarchar](200) NULL,
	[Organization] [nvarchar](200) NULL,
	[Id1] [nvarchar](100) NULL,
	[Id2] [nvarchar](100) NULL,
	[Id3] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[Street] [nvarchar](200) NULL,
	[Zip] [nvarchar](30) NULL,
	[State] [nvarchar](100) NULL,
	[Phone] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
 CONSTRAINT [PK_tShpLastOrderAddress] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[tShpLastOrderAddress]  WITH CHECK ADD  CONSTRAINT [FK_tShpLastOrderAddress_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[tAccount] ([AccountId])
GO
ALTER TABLE [dbo].[tShpLastOrderAddress] CHECK CONSTRAINT [FK_tShpLastOrderAddress_Account]
GO
