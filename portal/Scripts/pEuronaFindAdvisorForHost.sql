ALTER PROCEDURE [dbo].[pEuronaFindAdvisorForHost]
	@InstanceId INT = NULL,
	@Name NVARCHAR(100) = NULL,
	@City NVARCHAR(100) = NULL,
	@Region NVARCHAR(100) = NULL
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OrganizationId INT
	SELECT TOP 1 @OrganizationId=o.OrganizationId 
	FROM vOrganizations o
		LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
	WHERE 
		o.InstanceId=@InstanceId AND
		o.TopManager=1 AND
		( @Name IS NULL OR o.Name LIKE '%'+ @Name +'%') AND 
		( @City IS NULL OR ra.City LIKE @City +'%') AND 
		( @Region IS NULL OR o.RegionCode=@Region ) 
	ORDER BY o.SelectedCount ASC

	IF @OrganizationId IS NULL
	BEGIN
		SELECT TOP 1 @OrganizationId=op.OrganizationId 
		FROM vOrganizations o
			INNER JOIN vOrganizations op (NOLOCK) ON op.TVD_Id = o.ParentId
			LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
		WHERE 
			o.InstanceId=@InstanceId AND
			op.TopManager=1 AND
			( @Name IS NULL OR o.Name LIKE '%'+ @Name +'%') AND 
			( @City IS NULL OR ra.City LIKE @City +'%') AND 
			( @Region IS NULL OR o.RegionCode=@Region ) 
		ORDER BY op.SelectedCount ASC

	END

	-- RESULT
	SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
			o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
			o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
			RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
			CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
			InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
			BankContactId = o.BankContactId,
			ContactPersonId = o.ContactPersonId, 
			ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
			ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
			o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
			o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
			o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
			o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ
	FROM vOrganizations o
	LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
	LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
	LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
	LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
	WHERE OrganizationId=@OrganizationId AND @OrganizationId IS NOT NULL
	ORDER BY o.Name ASC
END
