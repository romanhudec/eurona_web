ALTER VIEW [dbo].[vAdvisorAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles,
	Created = ISNULL(a.Created, GETDATE()),
	AdvisorCode = o.Code, o.Name, Phone = o.ContactPhone, Mobile = o.ContactMobile,
	RegisteredAddress = (ar.Street + ', ' + ar.Zip + ', ' + ar.City +', ' + ar.State ),
	CorrespondenceAddress = (cr.Street + ', ' + cr.Zip + ', ' + cr.City +', ' + cr.State ),
	ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac (NOLOCK) ON ac.AccountId = a.AccountId
	INNER JOIN vOrganizations o (NOLOCK) ON o.AccountId = a.AccountId
	LEFT JOIN vAddresses ar (NOLOCK) ON ar.AddressId = o.RegisteredAddressId
	LEFT JOIN vAddresses cr (NOLOCK) ON cr.AddressId = o.CorrespondenceAddressId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]

GO