------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.6 to 0.7
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
-- TABLES
------------------------------------------------------------------------------------
ALTER TABLE tAccount ADD InstanceId INT NULL
GO
ALTER TABLE tPerson ADD InstanceId INT NULL
GO
ALTER TABLE tRole ADD InstanceId INT NULL
GO
ALTER TABLE tAccountRole ADD InstanceId INT NULL
GO
ALTER TABLE tBankContact ADD InstanceId INT NULL
GO
ALTER TABLE tOrganization ADD InstanceId INT NULL
GO
ALTER TABLE tFaq ADD InstanceId INT NULL
GO
ALTER TABLE tMasterPage ADD InstanceId INT NULL
GO
ALTER TABLE tUrlAlias ADD InstanceId INT NULL
GO
ALTER TABLE tPage ADD InstanceId INT NULL
GO
ALTER TABLE tMenu ADD InstanceId INT NULL
GO
ALTER TABLE tNavigationMenu ADD InstanceId INT NULL
GO
ALTER TABLE tNavigationMenuItem ADD InstanceId INT NULL
GO
ALTER TABLE tNews ADD InstanceId INT NULL
GO
ALTER TABLE tPoll ADD InstanceId INT NULL
GO
ALTER TABLE tPollOption ADD InstanceId INT NULL
GO
ALTER TABLE tPollAnswer ADD InstanceId INT NULL
GO
ALTER TABLE tProvidedService ADD InstanceId INT NULL
GO
ALTER TABLE tAccountCredit ADD InstanceId INT NULL
GO
ALTER TABLE tNewsletter ADD InstanceId INT NULL
GO
ALTER TABLE tIPNF ADD InstanceId INT NULL
GO
ALTER TABLE tVocabulary ADD InstanceId INT NULL
GO
ALTER TABLE tTranslation ADD InstanceId INT NULL
GO
ALTER TABLE tAccountVote ADD InstanceId INT NULL
GO
ALTER TABLE tTag ADD InstanceId INT NULL
GO
ALTER TABLE tComment ADD InstanceId INT NULL
GO
ALTER TABLE tArticle ADD InstanceId INT NULL
GO
ALTER TABLE tArticleComment ADD InstanceId INT NULL
GO
ALTER TABLE tBlog ADD InstanceId INT NULL
GO
ALTER TABLE tBlogComment ADD InstanceId INT NULL
GO
ALTER TABLE tProfile ADD InstanceId INT NULL
GO
ALTER TABLE tAccountProfile ADD InstanceId INT NULL
GO
ALTER TABLE tImageGallery ADD InstanceId INT NULL
GO
ALTER TABLE tImageGalleryComment ADD InstanceId INT NULL
GO
ALTER TABLE tImageGalleryItem ADD InstanceId INT NULL
GO
ALTER TABLE tImageGalleryItemComment ADD InstanceId INT NULL
GO
-- Classifiers
ALTER TABLE tAddress ADD InstanceId INT NULL
GO
ALTER TABLE cPaidService ADD InstanceId INT NULL
GO
ALTER TABLE cArticleCategory ADD InstanceId INT NULL
GO
ALTER TABLE cUrlAliasPrefix ADD InstanceId INT NULL
GO

------------------------------------------------------------------------------------
-- UPDATE TABLES
------------------------------------------------------------------------------------
UPDATE tAccount SET InstanceId = 1
GO
UPDATE tPerson SET InstanceId = 1
GO
UPDATE tRole SET InstanceId = 1
GO
UPDATE tAccountRole SET InstanceId = 1
GO
UPDATE tBankContact SET InstanceId = 1
GO
UPDATE tOrganization SET InstanceId = 1
GO
UPDATE tFaq SET InstanceId = 1
GO
UPDATE tMasterPage SET InstanceId = 1
GO
UPDATE tUrlAlias SET InstanceId = 1
GO
UPDATE tPage SET InstanceId = 1
GO
UPDATE tMenu SET InstanceId = 1
GO
UPDATE tNavigationMenu SET InstanceId = 1
GO
UPDATE tNavigationMenuItem SET InstanceId = 1
GO
UPDATE tNews SET InstanceId = 1
GO
UPDATE tPoll SET InstanceId = 1
GO
UPDATE tPollOption SET InstanceId = 1
GO
UPDATE tPollAnswer SET InstanceId = 1
GO
UPDATE tProvidedService SET InstanceId = 1
GO
UPDATE tAccountCredit SET InstanceId = 1
GO
UPDATE tNewsletter SET InstanceId = 1
GO
UPDATE tIPNF SET InstanceId = 1
GO
UPDATE tVocabulary SET InstanceId = 1
GO
UPDATE tTranslation SET InstanceId = 1
GO
UPDATE tAccountVote SET InstanceId = 1
GO
UPDATE tTag SET InstanceId = 1
GO
UPDATE tComment SET InstanceId = 1
GO
UPDATE tArticle SET InstanceId = 1
GO
UPDATE tArticleComment SET InstanceId = 1
GO
UPDATE tBlog SET InstanceId = 1
GO
UPDATE tBlogComment SET InstanceId = 1
GO
UPDATE tProfile SET InstanceId = 1
GO
UPDATE tAccountProfile SET InstanceId = 1
GO
UPDATE tImageGallery SET InstanceId = 1
GO
UPDATE tImageGalleryComment SET InstanceId = 1
GO
UPDATE tImageGalleryItem SET InstanceId = 1
GO
UPDATE tImageGalleryItemComment SET InstanceId = 1
GO
-- Classifiers
UPDATE tAddress SET InstanceId = 1
GO
UPDATE cPaidService SET InstanceId = 1
GO
UPDATE cArticleCategory SET InstanceId = 1
GO
UPDATE cUrlAliasPrefix SET InstanceId = 1
GO
------------------------------------------------------------------------------------
-- VIEWS
------------------------------------------------------------------------------------
ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	Roles = dbo.fAccountRoles(a.AccountId)
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	HistoryId IS NULL
ORDER BY [Login]
GO

/*
SELECT * FROM vAccounts
SELECT * FROM vAccountRoles

DECLARE @Roles NVARCHAR(200)
SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=0
SELECT @Roles
*/
------------------------------------------------------------------------------------
ALTER VIEW vPersons
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.PersonId, p.InstanceId, p.AccountId, p.Title, p.LastName, p.FirstName, ISNULL(p.Email, a.EMail) as Email, p.Notes,
	p.Phone, p.Mobile, p.AddressHomeId, p.AddressTempId
FROM
	tPerson p LEFT JOIN
	tAccount a ON a.AccountId = p.AccountId	
WHERE
	p.HistoryId IS NULL
ORDER BY p.LastName, p.FirstName
GO

-- SELECT * FROM vPersons
------------------------------------------------------------------------------------
ALTER VIEW vRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[RoleId], [InstanceId], [Name], [Notes]
FROM tRole
GO
------------------------------------------------------------------------------------
ALTER VIEW vAccountRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.InstanceId, ar.AccountRoleId, r.[RoleId], RoleName = r.[Name]
FROM tRole r
INNER JOIN tAccountRole ar (NOLOCK) ON ar.RoleId = r.RoleId
INNER JOIN tAccount a (NOLOCK) ON ar.AccountId = a.AccountId
GO
-- SELECT * FROM vAccountRoles
------------------------------------------------------------------------------------
ALTER VIEW vBankContacts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[BankContactId], [InstanceId], [BankName], [BankCode], [AccountNumber], [IBAN], [SWIFT]
FROM
	tBankContact b
WHERE
	b.HistoryId IS NULL
GO
--SELECT * FROM vBankContacts
------------------------------------------------------------------------------------
ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
	AccountId = o.AccountId,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

--SELECT * FROM vOrganizations
------------------------------------------------------------------------------------
ALTER VIEW vFaqs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[FaqId], [InstanceId], [Locale], [Order], [Question], [Answer]
FROM
	tFaq
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vFaqs
------------------------------------------------------------------------------------
ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [InstanceId], [Name], [Description], [Url]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages
------------------------------------------------------------------------------------
ALTER VIEW vUrlAliases
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[UrlAliasId], [InstanceId], [Url], [Locale], [Alias], [Name]
FROM tUrlAlias
GO

-- SELECT * FROM vUrlAliases
------------------------------------------------------------------------------------
ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PageId], p.[InstanceId], p.[MasterPageId], p.[Locale], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
	a.Url, a.Alias
FROM
	tPage p LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO

-- SELECT * FROM vPages
------------------------------------------------------------------------------------
ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.InstanceId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO
-- SELECT * FROM vMenu
------------------------------------------------------------------------------------
ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuId, m.InstanceId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu
------------------------------------------------------------------------------------
ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuItemId, m.InstanceId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenuItem m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenuItem
------------------------------------------------------------------------------------
ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	n.[NewsId], n.InstanceId, n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
	n.UrlAliasId, alias.Alias, alias.Url
FROM
	tNews n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId

WHERE
	n.HistoryId IS NULL 
GO

-- SELECT * FROM vNews
------------------------------------------------------------------------------------
ALTER VIEW vPolls
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PollId], p.InstanceId, p.[Closed], p.[Locale], p.[Question], p.[DateFrom], p.[DateTo], p.[Icon],
	VotesTotal = ( SELECT SUM(Votes) FROM vPollOptions WHERE PollId = p.PollId )
FROM
	tPoll p
WHERE
	p.HistoryId IS NULL
GO

-- SELECT * FROM vPools
------------------------------------------------------------------------------------
ALTER VIEW vPollOptions
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	o.[PollOptionId], o.InstanceId, o.[PollId], o.[Order], o.[Name], 
	Votes = (SELECT COUNT(*) FROM tPollAnswer WHERE PollOptionId = o.[PollOptionId] )
FROM
	tPollOption o 
GO

-- SELECT * FROM vPollOptions
------------------------------------------------------------------------------------
ALTER VIEW vPollAnswers
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[PollAnswerId], [InstanceId], [PollOptionId], [IP]
FROM
	tPollAnswer
GO

-- SELECT * FROM vPollAnswers
------------------------------------------------------------------------------------
ALTER VIEW vProvidedServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ps.[ProvidedServiceId], ps.InstanceId, ps.[AccountId], ps.[PaidServiceId], ps.ObjectId, ps.[ServiceDate], p.CreditCost, p.[Name], p.[Notes]
FROM
	tProvidedService ps INNER JOIN
	vPaidServices p ON p.PaidServiceId = ps.PaidServiceId
GO
------------------------------------------------------------------------------------
ALTER VIEW vAccountsCredit
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	[AccountCreditId], [InstanceId], [AccountId], [Credit]
FROM
	tAccountCredit
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vNewsletter
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[NewsletterId], [InstanceId], [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles], [SendDate]
FROM
	tNewsletter
GO

-- SELECT * FROM vNewsletter
------------------------------------------------------------------------------------
ALTER VIEW vIPNFs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[IPNFId], [InstanceId], [Type], [Locale], [IPF], [Notes]
FROM tIPNF
GO
------------------------------------------------------------------------------------
ALTER VIEW vVocabularies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[VocabularyId], [InstanceId], [Locale], [Name], [Notes]
FROM tVocabulary
GO

-- SELECT * FROM vVocabularies
------------------------------------------------------------------------------------
ALTER VIEW vTranslations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.VocabularyId, v.InstanceId, VocabularyName = v.Name, v.Locale, t.TranslationId, t.Term, t.Translation, t.Notes
FROM tTranslation t (NOLOCK)
INNER JOIN tVocabulary v (NOLOCK) ON t.VocabularyId = v.VocabularyId
WHERE t.HistoryId IS NULL
GO

-- SELECT * FROM vTranslations
------------------------------------------------------------------------------------
ALTER VIEW vAccountVotes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	AccountVoteId, InstanceId, ObjectType, ObjectId, AccountId, Rating, [Date]
FROM tAccountVote
GO
------------------------------------------------------------------------------------
ALTER VIEW vTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	TagId, InstanceId, Tag
FROM tTag WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[CommentId], [InstanceId], [ParentId], [AccountId], [Date], [Title], [Content], [Votes], [TotalRating]
FROM
	tComment
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vArticles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleId, a.InstanceId, a.Locale, a.Icon, a.Title, a.Teaser, a.Content, a.RoleId, a.Country,
	a.ArticleCategoryId, ArticleCategoryName = c.Name,
	a.City, a.Approved, a.ReleaseDate, a.ExpiredDate, 
	a.EnableComments, a.Visible, 
	CommentsCount = ( SELECT Count(*) FROM vArticleComments WHERE ArticleId = a.ArticleId ),
	ViewCount = ISNULL(a.ViewCount, 0 ), 
	Votes = ISNULL(a.Votes, 0), 
	TotalRating = ISNULL(a.TotalRating, 0),
	RatingResult =  ISNULL(a.TotalRating*1.0/a.Votes*1.0, 0 ),
	a.UrlAliasId, alias.Alias, alias.Url

FROM
	tArticle a INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = a.UrlAliasId

WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vArticleComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.ArticleCommentId, ac.InstanceId, ac.ArticleId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tArticleComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------
ALTER VIEW vBlogs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	b.BlogId, b.InstanceId, b.Locale, b.Icon, b.Title, b.Teaser, b.Content, b.RoleId, b.Country,
	b.AccountId, Login = a.Login,
	b.City, b.Approved, b.ReleaseDate, b.ExpiredDate, 
	b.EnableComments, b.Visible, 
	b.UrlAliasId, alias.Alias, alias.Url,
	CommentsCount = ( SELECT Count(*) FROM vBlogComments WHERE BlogId = b.BlogId ),
	ViewCount = ISNULL(b.ViewCount, 0 ), 
	Votes = ISNULL(b.Votes, 0), 
	TotalRating = ISNULL(b.TotalRating, 0),
	RatingResult =  ISNULL(b.TotalRating*1.0/b.Votes*1.0, 0 )
FROM
	tBlog b INNER JOIN vAccounts a ON a.AccountId = b.AccountId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = b.UrlAliasId
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vBlogComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.BlogCommentId, ac.InstanceId, ac.BlogId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tBlogComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------
ALTER VIEW vProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ProfileId, InstanceId, [Name], [Type], [Description]
FROM tProfile
GO
------------------------------------------------------------------------------------
ALTER VIEW vAccountProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ap.AccountProfileId, ap.InstanceId, ap.AccountId, ap.ProfileId, ap.[Value], ProfileType = p.Type, ProfileName = p.Name
FROM tAccountProfile ap 
INNER JOIN tProfile p ON p.ProfileId = ap.ProfileId
WHERE ap.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vImageGalleries
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryId, g.InstanceId, g.RoleId, g.[Name], g.[Date], g.EnableComments, g.EnableVotes, g.Visible,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryComments WHERE ImageGalleryId = g.ImageGalleryId  ),
	ItemsCount = ( SELECT Count(*) FROM vImageGalleryItems WHERE ImageGalleryId = g.ImageGalleryId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	g.UrlAliasId, a.Alias, a.Url
	
FROM tImageGallery g LEFT JOIN tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igc.ImageGalleryCommentId, igc.InstanceId, igc.ImageGalleryId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryComment igc 
	INNER JOIN vComments c ON c.CommentId = igc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryItems
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ImageGalleryItemId, InstanceId, ImageGalleryId, [VirtualPath], [VirtualThumbnailPath], [Position], [Date], Description,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryItemComments WHERE ImageGalleryItemId = g.ImageGalleryItemId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	Votes = ISNULL(Votes, 0), 
	TotalRating = ISNULL(TotalRating, 0),
	RatingResult =  ISNULL(TotalRating*1.0/Votes*1.0, 0 )	
FROM tImageGalleryItem g
WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryItemComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igic.ImageGalleryItemCommentId, igic.InstanceId, igic.ImageGalleryItemId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryItemComment igic 
	INNER JOIN vComments c ON c.CommentId = igic.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------
ALTER VIEW vAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [InstanceId], [City], [Street], [Zip], [Notes], [District], [Region], [Country], [State]
FROM
	tAddress
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vAddresses
------------------------------------------------------------------------------------
ALTER VIEW vPaidServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	PaidServiceId, [InstanceId], [Name], [Notes], [CreditCost]
FROM
	cPaidService
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vArticleCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	a.ArticleCategoryId, a.[InstanceId], a.[Name], a.[Code], a.[Locale], a.[Notes], 
	ArticlesInCategory = (SELECT Count(*) FROM tArticle 
		WHERE HistoryId IS NULL AND
			  Visible=1 AND 
			  ReleaseDate<=GETDATE() AND 
			  ArticleCategoryId = a.ArticleCategoryId )
FROM
	cArticleCategory a
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vUrlAliasPrefixes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	UrlAliasPrefixId, [InstanceId], [Name], [Code], [Locale], [Notes]
FROM cUrlAliasPrefix
WHERE HistoryId IS NULL
GO

------------------------------------------------------------------------------------
-- PROCEDURES
------------------------------------------------------------------------------------
ALTER PROCEDURE pAddressCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@City NVARCHAR(100) = '',
	@District NVARCHAR(100) = '',
	@Region NVARCHAR(100) = '',
	@Country NVARCHAR(100) = '',
	@State NVARCHAR(100)= '',	
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAddress ( InstanceId, City, Street, Zip, District, Region, Country, State, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @City, @Street, @Zip, @District, @Region, @Country, @State, @Notes,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AddressId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Locale [char](2) = 'en', 
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cArticleCategory ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Code, @Notes, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleCategoryId = @Result

END
GO
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		RETURN
	END
	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified],
		HistoryStamp, HistoryType, HistoryAccount)
	VALUES (@InstanceId, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified,
		GETDATE(), 'C', @HistoryAccount)
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole ( InstanceId, AccountId, RoleId)
		SELECT @InstanceId, @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END
GO

-- EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'aaa', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68' -- @Password=0987oiuk
------------------------------------------------------------------------------------
ALTER PROCEDURE pPersonCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT = NULL,
	@Title NVARCHAR(20) = '',
	@FirstName NVARCHAR(100) = '',
	@LastName NVARCHAR(100) = '',
	@Email NVARCHAR(100) = '',
	@Phone NVARCHAR(100) = NULL, @Mobile NVARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @AddressHomeId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @Result = @AddressHomeId OUTPUT

		DECLARE @AddressTempId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @Result = @AddressTempId OUTPUT
		

		INSERT INTO tPerson ( InstanceId, AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount)
		VALUES ( @InstanceId, @AccountId, @Title, @FirstName, @LastName, @Email,
			@Phone, @Mobile, @AddressHomeId, @AddressTempId,
			GETDATE(), 'C', @HistoryAccount)
			
		SET @Result = SCOPE_IDENTITY()
	
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

		SET @Result = NULL

	END CATCH	
	
	SELECT PersonId = @Result

END
GO

/*
DECLARE @Result INT
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'hudy', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pPersonCreate @HistoryAccount = 1, @InstanceId=1, @AccountId = @Result, @FirstName='Roman', @LastName='Hudec', @Result = @Result OUTPUT
SELECT * FROM tPerson
SELECT * FROM tAccount
*/
------------------------------------------------------------------------------------
ALTER PROCEDURE pRoleCreate
	@InstanceId INT,
	@Name NVARCHAR(200),
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tRole ( InstanceId, [Name], [Notes] ) VALUES ( @InstanceId, @Name, @Notes )
	SET @Result = SCOPE_IDENTITY()

	SELECT RoleId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pBankContactCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@BankName NVARCHAR(100) = '',
	@BankCode NVARCHAR(100) = '',
	@AccountNumber NVARCHAR(100) = '',
	@IBAN NVARCHAR(100) = '',
	@SWIFT NVARCHAR(100) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBankContact ( InstanceId, AccountNumber, BankName, BankCode, SWIFT, IBAN,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountNumber, @BankName, @BankCode, @SWIFT, @IBAN,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BankContactId = @Result
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pOrganizationCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT = NULL,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @RegisteredAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @RegisteredAddressId OUTPUT

		DECLARE @CorrespondenceAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @CorrespondenceAddressId OUTPUT
		
		DECLARE @InvoicingAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @InvoicingAddressId OUTPUT

		DECLARE @BankContactId INT
		EXEC pBankContactCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @BankContactId OUTPUT

		DECLARE @ContactPersonId INT
		EXEC pPersonCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @ContactPersonId OUTPUT

		INSERT INTO tOrganization (
			InstanceId, AccountId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			GETDATE(), 'C', @HistoryAccount)
			
		SET @Result = SCOPE_IDENTITY()

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

	SELECT OrganizationId = @Result

END
GO

/*
DECLARE @Result INT
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'mothiva', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pOrganizationCreate @HistoryAccount=1, @InstanceId=1, @AccountId=@Result, @Name='Mothiva, s.r.o.'

SELECT * FROM tAccount
SELECT * from vOrganizations
*/
------------------------------------------------------------------------------------
ALTER PROCEDURE pFaqCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tFaq ( InstanceId, Locale, [Order], Question, Answer,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Order, @Question, @Answer,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT FaqId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasCreate
	@InstanceId INT,
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Url, @Locale, @InstanceId);
		RETURN
	END	

	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias ( InstanceId, Url, Locale, Alias, [Name] ) 
	VALUES ( @InstanceId, @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage ( InstanceId, MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( InstanceId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT MenuId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenu ( InstanceId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NavigationMenuId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuItemCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenuItem ( InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @NavigationMenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NavigationMenuItemId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pNewsCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@UrlAliasId INT = NULL,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( InstanceId, Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Date, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pPollCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Closed BIT = 0,
	@Locale [char](2) = 'en', 
	@Question NVARCHAR(1000),
	@DateFrom DATETIME,
	@DateTo DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPoll ( InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Closed, @Locale, @Question, @DateFrom, @DateTo, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PollId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pPollOptionCreate
	@PollId INT,
	@InstanceId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollOption ( InstanceId, PollId, [Order], [Name] )
	VALUES ( @InstanceId, @PollId, @Order, @Name )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollOptionId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pPollAnswerCreate
	@PollOptionId INT,
	@InstanceId INT,
	@IP NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollAnswer ( InstanceId, PollOptionId, IP ) 
	VALUES ( @InstanceId, @PollOptionId, @IP )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollAnswerId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountCreditCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@Credit DECIMAL(19,2),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountCredit ( InstanceId, AccountId, Credit, HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @Credit, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountCreditId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pProvidedServiceCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@PaidServiceId INT,
	@ObjectId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
	INSERT INTO tProvidedService ( InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate ) 
	VALUES ( @InstanceId, @AccountId, @PaidServiceId, @ObjectId, GETDATE() )
	SET @Result = SCOPE_IDENTITY()
	
	DECLARE @CreditCost DECIMAL(19,2)
	SELECT @CreditCost = CreditCost FROM vPaidServices WHERE PaidServiceId = @PaidServiceId
	
	--Update aktualny kredit pouzivatela
	IF @CreditCost IS NOT NULL
	BEGIN
		DECLARE @AccountCreditId INT, @CurrentCredit DECIMAL(19,2), @NewCredit DECIMAL(19,2)
		SET @NewCredit = @CreditCost*(-1)
		-- A neexistuje zaznam o kredite pouzivatela, vytvorim ho a odpocitam jeho credit od aktualneho kreditu
		SELECT @AccountCreditId=AccountCreditId, @CurrentCredit=Credit FROM vAccountsCredit WHERE AccountId=@AccountId 	
		IF @AccountCreditId IS NULL
		BEGIN
			EXEC pAccountCreditCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @AccountId = @AccountId, @Credit=@NewCredit
		END	
		ELSE
		BEGIN
			SET @NewCredit = ( @CurrentCredit - @CreditCost)
			EXEC pAccountCreditModify @HistoryAccount = @HistoryAccount, @AccountCreditId = @AccountCreditId, @Credit=@NewCredit
		END

	END	
	
	COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

		SET @Result = NULL

	END CATCH	

	SELECT ProvidedServiceId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pNewsletterCreate
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Subject NVARCHAR(255) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Attachment IMAGE = NULL,
	@SendDate DATETIME = NULL,
	@Roles NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNewsletter ( InstanceId, Locale, [Date], Icon, Subject, Attachment, Content, Roles, SendDate )
	VALUES ( @InstanceId, @Locale, @Date, @Icon, @Subject, @Attachment, @Content, @Roles, @SendDate )

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsletterId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pTranslationCreateEx
	@HistoryAccount INT,
	@InstanceId INT,
	@Vocabulary NVARCHAR(100),
	@Locale CHAR(2),
	@Term NVARCHAR(500), 
	@Translation NVARCHAR(4000), 
	@Notes NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @VocabularyId INT
		SELECT @VocabularyId = VocabularyId FROM vVocabularies WHERE Name = @Vocabulary AND Locale = @Locale		
		IF @VocabularyId IS NULL BEGIN
			INSERT INTO tVocabulary( InstanceId, Locale, Name, Notes) VALUES ( @InstanceId, @Locale, @Vocabulary, '')
			SET @VocabularyId = SCOPE_IDENTITY()
		END

		DECLARE @TranslationId INT
		SELECT @TranslationId = TranslationId FROM vTranslations WHERE VocabularyId = @VocabularyId AND Term = @Term		
		IF @TranslationId IS NULL BEGIN
			INSERT INTO tTranslation( InstanceId, VocabularyId, Term, Translation, Notes,
				HistoryStamp, HistoryType, HistoryAccount, HistoryId) 
			VALUES ( @InstanceId, @VocabularyId, @Term, @Translation, @Notes,
				GETDATE(), 'C', @HistoryAccount, NULL)
			SET @TranslationId = SCOPE_IDENTITY()
		END

		SET @Result = @TranslationId

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
------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountVoteCreate
	@InstanceId INT,
	@AccountId INT,
	@ObjectType INT,
	@ObjectId INT,
	@Rating INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountVote ( InstanceId, AccountId, ObjectType, ObjectId, Rating, [Date]) 
	VALUES ( @InstanceId, @AccountId, @ObjectType, @ObjectId, @Rating, GETDATE())

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountVoteId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pTagCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tTag ( InstanceId, Tag, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Tag, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT TagId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@Date DATETIME,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tComment ( InstanceId, ParentId, AccountId, [Date], Title, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @AccountId, @Date, @Title, @Content, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CommentId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ArticleCategoryId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tArticle ( InstanceId, ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ArticleCategoryId, @Locale, @Icon, @Title, @Teaser, @ContentKeywords, @Content, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ArticleId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tArticleComment ( InstanceId, CommentId, ArticleId ) VALUES ( @InstanceId, @CommentId, @ArticleId )

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBlog ( InstanceId, AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @Locale, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BlogId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@BlogId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tBlogComment ( InstanceId, CommentId, BlogId ) VALUES ( @InstanceId, @CommentId, @BlogId )

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountProfileCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( InstanceId, AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @ProfileId, @Value, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountProfileId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGallery ( InstanceId, [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @RoleId, @Visible, @UrlAliasId, ISNULL(@Date,GETDATE()), @EnableComments, @EnableVotes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ImageGalleryId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryComment ( InstanceId, CommentId, ImageGalleryId ) VALUES ( @InstanceId, @CommentId, @ImageGalleryId )

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGalleryItem ( InstanceId, ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ImageGalleryId, @VirtualPath, @VirtualThumbnailPath, @Position, GETDATE(), @Description,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryItemId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ImageGalleryItemId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryItemComment ( InstanceId, CommentId, ImageGalleryItemId ) VALUES ( @InstanceId, @CommentId, @ImageGalleryItemId )

END
GO
------------------------------------------------------------------------------------
-- SEARCH
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Title, 
		Content = n.Teaser + n.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND n.InstanceId = @InstanceId AND
	(
		n.Title LIKE '%'+@Keywords+'%' OR 
		n.Teaser LIKE '%'+@Keywords+'%' OR 
		n.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleryItemComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS,
	ImageUrl NVARCHAR(500)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias + '/' +  CAST(gc.ImageGalleryItemId AS NVARCHAR),
		ImageUrl = gi.VirtualThumbnailPath
		FROM vImageGalleryItemComments gc INNER JOIN
		tImageGalleryItem gi ON gi.ImageGalleryItemId = gc.ImageGalleryItemId INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gi.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleryComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS,
	ImageUrl NVARCHAR(500)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias,
		ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = g.ImageGalleryId ORDER BY gi.Position ASC) 
		FROM vImageGalleryComments gc INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gc.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleries
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = i.ImageGalleryId, Title = i.Name, Content = NULL, UrlAlias = a.Alias,
	ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = i.ImageGalleryId ORDER BY gi.Position ASC) 
	FROM tImageGallery i INNER JOIN
	tUrlAlias a ON a.UrlAliasId = i.UrlAliasId
	WHERE i.HistoryId IS NULL AND i.InstanceId = @InstanceId AND
	(
		i.Name LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogs
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = b.BlogId, b.Title, 
		Content = b.Teaser + b.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tBlog b INNER JOIN
	tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
	WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
	(
		b.Title LIKE '%'+@Keywords+'%' OR 
		b.Teaser LIKE '%'+@Keywords+'%' OR 
		b.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vBlogComments gc INNER JOIN
		tBlog b ON b.BlogId = gc.BlogId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
		WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticles
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = art.ArticleId, art.Title, 
		Content = art.Teaser + art.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tArticle art INNER JOIN
	tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
	WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
	(
		art.Title LIKE '%'+@Keywords+'%' OR 
		art.Teaser LIKE '%'+@Keywords+'%' OR 
		art.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchPages
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.PageId, p.Title, Content = p.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tPage p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticleComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vArticleComments gc INNER JOIN
		tArticle art ON art.ArticleId = gc.ArticleId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
		WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Upgrade CMS db version
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 7, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
