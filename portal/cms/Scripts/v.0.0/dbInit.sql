------------------------------------------------------------------------------------------------------------------------
-- Init
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- default account & credentials

SET IDENTITY_INSERT tRole ON
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-1, 'Administrator', 'System administrator')
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-2, 'Newsletter', 'Information bulletin')
SET IDENTITY_INSERT tRole OFF

EXEC pAccountCreate @HistoryAccount = NULL,
	@Login = 'system', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', -- @Password=0987oiuk
	@Roles = 'Administrator', @Verified = 1

GO

-- EOF Init
------------------------------------------------------------------------------------------------------------------------
