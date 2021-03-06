------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- classifiers
CREATE VIEW vAddresses AS SELECT A=1
GO
CREATE VIEW vUrlAliasPrefixes AS SELECT A=1
GO
CREATE VIEW vSupportedLocales AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- permissions

CREATE VIEW vRoles AS SELECT A=1
GO

CREATE VIEW vAccountRoles AS SELECT AccountId=-1, RoleName=''
GO

------------------------------------------------------------------------------------------------------------------------
-- OS&Persons
CREATE VIEW vAccounts AS SELECT AccountId=1, Email=''
GO

CREATE VIEW vPersons AS SELECT A=1
GO

CREATE VIEW vBankContacts AS SELECT BankContactId=1
GO

CREATE VIEW vOrganizations AS SELECT OrganizationId=1, [Name]=''
GO

------------------------------------------------------------------------------------------------------------------------
-- 

CREATE VIEW vMasterPages AS SELECT A=1
GO

CREATE VIEW vUrlAliases AS SELECT A=1
GO

CREATE VIEW vPages AS SELECT A=1
GO

CREATE VIEW vMenu AS SELECT A=1
GO

CREATE VIEW vNavigationMenu AS SELECT A=1
GO

CREATE VIEW vNavigationMenuItem AS SELECT A=1
GO

CREATE VIEW vFaqs AS SELECT A=1
GO

CREATE VIEW vNews AS SELECT A=1
GO

CREATE VIEW vNewsletter AS SELECT A=1
GO

CREATE VIEW vPolls AS SELECT A=1
GO

CREATE VIEW vPollOptions AS SELECT A=1
GO

CREATE VIEW vPollAnswers AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- CREDIT MANAGEMENT
CREATE VIEW vPaidServices AS SELECT A=1
GO
CREATE VIEW vAccountsCredit AS SELECT AccountId=1, Credit=0
GO
CREATE VIEW vProvidedServices AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- IPNF
CREATE VIEW vIPNFs AS SELECT A=1
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & translation

CREATE VIEW vVocabularies AS SELECT A=1
GO

CREATE VIEW vTranslations AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- AccountVotes
CREATE VIEW vAccountVotes AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Tags
CREATE VIEW vTags AS SELECT TagId=1, Tag=1
GO
CREATE VIEW vArticleTags AS SELECT TagId=1, ArticleId=1
GO
CREATE VIEW vBlogTags AS SELECT TagId=1, BlogId=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Comment
CREATE VIEW vComments AS SELECT CommentId=1, ParentId=1, AccountId=1, Date=1, Title=1, Content=1, Votes=1, TotalRating=1
GO
CREATE VIEW vArticleComments AS SELECT A=1
GO
CREATE VIEW vBlogComments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Articles
CREATE VIEW vArticleCategories AS SELECT A=1
GO
CREATE VIEW vArticles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Blogs
CREATE VIEW vBlogs AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Profile
CREATE VIEW vProfiles AS SELECT A=1
GO
CREATE VIEW vAccountProfiles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGallery
CREATE VIEW vImageGalleries AS SELECT A=1
GO
CREATE VIEW vImageGalleryTags AS SELECT TagId=1, ImageGalleryId=1
GO
CREATE VIEW vImageGalleryComments AS SELECT A=1
GO
CREATE VIEW vImageGalleryItems AS SELECT ImageGalleryId=1
GO
CREATE VIEW vImageGalleryItemComments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Views declarations
------------------------------------------------------------------------------------------------------------------------
