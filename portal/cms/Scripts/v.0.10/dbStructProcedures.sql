------------------------------------------------------------------------------------------------------------------------
-- Procedures declarations
------------------------------------------------------------------------------------------------------------------------
-- address

CREATE PROCEDURE pAddressCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAddressModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAddressDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Supported locale
CREATE PROCEDURE pSupportedLocaleCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- url alias prefix
CREATE PROCEDURE pUrlAliasPrefixModify AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- roles
CREATE PROCEDURE pRoleCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pRoleModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pRoleDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- person and accounts
CREATE PROCEDURE pAccountCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountVerify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Bank contact

CREATE PROCEDURE pBankContactCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBankContactModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBankContactDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Organization

CREATE PROCEDURE pOrganizationCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pOrganizationModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pOrganizationDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- UrlAlias
CREATE PROCEDURE pUrlAliasCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Page
CREATE PROCEDURE pPageCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pPageModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pPageDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Menu
CREATE PROCEDURE pMenuCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pMenuModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pMenuDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenu
CREATE PROCEDURE pNavigationMenuCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenuItem
CREATE PROCEDURE pNavigationMenuItemCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuItemModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuItemDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Faq
CREATE PROCEDURE pFaqCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pFaqModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pFaqDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- News
CREATE PROCEDURE pNewsCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Newsletter
CREATE PROCEDURE pNewsletterCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsletterModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsletterDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Poll
CREATE PROCEDURE pPollCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollDelete AS BEGIN SET NOCOUNT ON; END
GO

-- PollOption
CREATE PROCEDURE pPollOptionCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollOptionModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollOptionDelete AS BEGIN SET NOCOUNT ON; END
GO

-- PollAnswer
CREATE PROCEDURE pPollAnswerCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- CREDIT management
CREATE PROCEDURE pPaidServiceModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountCreditCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountCreditModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountCreditDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pProvidedServiceCreate AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & Translation

-- iba tato procka je urcena pre UI. User si moze len upravit text
CREATE PROCEDURE pTranslationModify AS BEGIN SET NOCOUNT ON; END
GO

-- developerska procka pre zalozenie noveho textu v scripte
CREATE PROCEDURE pTranslationCreateEx AS BEGIN SET NOCOUNT ON; END
GO

-- TODO: zamysliet sa nad pTransaltionDeleteEx... ak sa zrusi niekde text, mat ho moznost vymazat... developer si musi byt isty ze text je mozne vymazat

------------------------------------------------------------------------------------------------------------------------
-- AccountVotes
CREATE PROCEDURE pAccountVoteCreate AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Tags
CREATE PROCEDURE pTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Comments
CREATE PROCEDURE pCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pCommentIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ArticleCategory
CREATE PROCEDURE pArticleCategoryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Article
CREATE PROCEDURE pArticleCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pArticleIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pArticleTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Blog
CREATE PROCEDURE pBlogCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- AccountProfile
CREATE PROCEDURE pAccountProfileCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGallery
CREATE PROCEDURE pImageGalleryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryItem
CREATE PROCEDURE pImageGalleryItemCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryItemIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Search Engine procedures
CREATE PROCEDURE pSearchPages AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchArticles AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogs AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchNews AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleries AS BEGIN SET NOCOUNT ON; END
GO
-- Comments
CREATE PROCEDURE pSearchArticleComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryItemComments AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Procedures declarations
------------------------------------------------------------------------------------------------------------------------
