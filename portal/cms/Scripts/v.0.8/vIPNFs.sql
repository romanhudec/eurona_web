
ALTER VIEW vIPNFs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[IPNFId], [InstanceId], [Type], [Locale], [IPF], [Notes]
FROM tIPNF
GO
