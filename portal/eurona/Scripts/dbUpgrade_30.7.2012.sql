------------------------------------------------------------------------------------------------------------------------
-- UPGRADE Eurona
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT, @ProductsFBMasterPageId INT
SET @MasterPageId = 1

DECLARE @UrlAliasId INT
DECLARE @PageId INT
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tOrganization ADD  Angel_team_manager_typ INT NULL DEFAULT(0)
GO

------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
	AccountId = o.AccountId, a.TVD_Id, a.Verified,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact,
	o.ParentId, o.Code, o.VATPayment, o.TopManager,
	o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin,  RestrictedAccess = ISNULL(o.RestrictedAccess, 0), o.Statut,
	Created = ( SELECT MIN(HistoryStamp) FROM tAccount WHERE ( AccountId=a.AccountId OR HistoryId=a.AccountId )  ),
	SelectedCount,
	AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
	Angel_team_clen, Angel_team_manager, Angel_team_manager_typ
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpProduct ADD [OnWeb] BIT NOT NULL default(1)
GO

ALTER VIEW [dbo].[vShpProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, BeznaCena = ISNULL(cp.BeznaCena, 0 ), MarzePovolena = ISNULL( cp.MarzePovolena, 0 ), MarzePovolenaMinimalni = ISNULL( cp.MarzePovolenaMinimalni, 0 ),
	cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , cp.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], 
	p.[Megasleva],  p.[Supercena], p.[CLHit], p.[Action], p.[Vyprodej], p.[OnWeb],
	p.Parfumacia, p.Discount, cp.DynamickaSleva, cp.StatickaSleva,
	p.VAT, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale, p.MaximalniPocetVBaleni, BonusovyKredit = cp.[CenaBK]
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL

GO

ALTER TABLE tShpProduct ADD [VamiNejviceNakupovane] INT NULL
GO

ALTER VIEW [dbo].[vShpProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, BeznaCena = ISNULL(cp.BeznaCena, 0 ), MarzePovolena = ISNULL( cp.MarzePovolena, 0 ), MarzePovolenaMinimalni = ISNULL( cp.MarzePovolenaMinimalni, 0 ),
	cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , cp.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], 
	p.[Megasleva],  p.[Supercena], p.[CLHit], p.[Action], p.[Vyprodej], p.[OnWeb],
	p.Parfumacia, p.Discount, cp.DynamickaSleva, cp.StatickaSleva, p.VamiNejviceNakupovane,
	p.VAT, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale, p.MaximalniPocetVBaleni, BonusovyKredit = cp.[CenaBK]
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductModifyEx
	@HistoryAccount INT,
	@ProductId INT,
	@MaximalniPocetVBaleni INT = NULL,
	@VamiNejviceNakupovane INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProduct
		SET
			MaximalniPocetVBaleni=@MaximalniPocetVBaleni, VamiNejviceNakupovane=@VamiNejviceNakupovane,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductId = @ProductId

		SET @Result = @ProductId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tAnonymniRegistrace ADD EuronaReistraceProcent INT NULL, EuronaReistracePocitadlo INT NOT NULL default(0)
GO
------------------------------------------------------------------------------------------------------------------------
--1.7.2012
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vLoggedAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[Login], a.Email, la.LoggedAt, a.TVD_Id, a.[InstanceId], o.Code, o.Name, LoggedMinutes=DATEDIFF(minute, la.LoggedAt , GETDATE()),
	o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ
FROM
	tLoggedAccount la 
	INNER JOIN tAccount a ON a.AccountId = la.AccountId
	LEFT JOIN tOrganization o ON o.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tBonusovyKredit ADD HodnotaOdSK DECIMAL(19,2) NULL, HodnotaDoSK DECIMAL(19,2) NULL,
 HodnotaOdPL DECIMAL(19,2) NULL, HodnotaDoPL DECIMAL(19,2) NULL
 GO
 ------------------------------------------------------------------------------------------------------------------------
 ALTER VIEW [dbo].[vBonusoveKredity]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	BonusovyKreditId, InstanceId, Typ, HodnotaOd, HodnotaDo, HodnotaOdSK, HodnotaDoSK, HodnotaOdPL, HodnotaDoPL, Kredit
FROM
	tBonusovyKredit

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pBonusovyKreditCreate]
	@InstanceId INT,
	@Typ INT = 0,
	@HodnotaOd DECIMAL(19,2) = NULL,
	@HodnotaDo DECIMAL(19,2) = NULL,
	@HodnotaOdSK DECIMAL(19,2) = NULL,
	@HodnotaDoSK DECIMAL(19,2) = NULL,
	@HodnotaOdPL DECIMAL(19,2) = NULL,
	@HodnotaDoPL DECIMAL(19,2) = NULL,
	@Kredit DECIMAL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO tBonusovyKredit ( InstanceId, Typ, HodnotaOd, HodnotaDo, HodnotaOdSK, HodnotaDoSK, HodnotaOdPL, HodnotaDoPL, Kredit )
	VALUES (@InstanceId, @Typ, @HodnotaOd, @HodnotaDo, @HodnotaOdSK, @HodnotaDoSK, @HodnotaOdPL, @HodnotaDoPL, @Kredit )
	
	SET @Result = SCOPE_IDENTITY()
	SELECT BonusovyKreditId = @Result

END

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pBonusovyKreditModify]
	@InstanceId INT,
	@BonusovyKreditId INT,
	@Typ INT = 0,
	@HodnotaOd DECIMAL(19,2) = NULL,
	@HodnotaDo DECIMAL(19,2) = NULL,
	@HodnotaOdSK DECIMAL(19,2) = NULL,
	@HodnotaDoSK DECIMAL(19,2) = NULL,
	@HodnotaOdPL DECIMAL(19,2) = NULL,
	@HodnotaDoPL DECIMAL(19,2) = NULL,
	@Kredit DECIMAL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
		
	IF NOT EXISTS(SELECT * FROM tBonusovyKredit WHERE BonusovyKreditId = @BonusovyKreditId  AND InstanceId=@InstanceId) BEGIN
		RAISERROR( 'Invalid BonusovyKreditId %d', 16, 1, @BonusovyKreditId );
		RETURN
	END
	
	UPDATE tBonusovyKredit SET Typ=@Typ, HodnotaOd=@HodnotaOd, Kredit=@Kredit,
	HodnotaDo=@HodnotaDo, HodnotaOdSK=@HodnotaOdSK, HodnotaDoSK=@HodnotaDoSK, HodnotaOdPL=@HodnotaOdPL, HodnotaDoPL=@HodnotaDoPL
	WHERE BonusovyKreditId = @BonusovyKreditId AND InstanceId=@InstanceId

	SET @Result = @BonusovyKreditId

END

GO
------------------------------------------------------------------------------------------------------------------------