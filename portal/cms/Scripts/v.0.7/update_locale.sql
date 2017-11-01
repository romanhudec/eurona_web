--ACCOunt
ALTER TABLE [tAccount] DROP CONSTRAINT [CK_tAccount_Locale]
GO
ALTER TABLE [tAccount]  WITH CHECK 
	ADD CONSTRAINT [CK_tAccount_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tAccount] CHECK CONSTRAINT [CK_tAccount_Locale]
GO
-- FAQ
ALTER TABLE [tFaq] DROP CONSTRAINT [CK_tFaq_Locale]
GO
ALTER TABLE [tFaq]  WITH CHECK 
	ADD CONSTRAINT [CK_tFaq_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tFaq] CHECK CONSTRAINT [CK_tFaq_Locale]
GO
--UrlAlias
ALTER TABLE [tUrlAlias] DROP CONSTRAINT [CK_tUrlAlias_Locale]
GO
ALTER TABLE [tUrlAlias]  WITH CHECK 
	ADD CONSTRAINT [CK_tUrlAlias_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tUrlAlias] CHECK CONSTRAINT [CK_tUrlAlias_Locale]
GO
--[tPage]
ALTER TABLE [tPage] DROP CONSTRAINT [CK_tPage_Locale]
GO
ALTER TABLE [tPage]  WITH CHECK 
	ADD CONSTRAINT [CK_tPage_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [CK_tPage_Locale]
GO
--[tMenu]
ALTER TABLE [tMenu] DROP CONSTRAINT [CK_tMenu_Locale]
GO
ALTER TABLE [tMenu]  WITH CHECK 
	ADD CONSTRAINT [CK_tMenu_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [CK_tMenu_Locale]
GO
--[tNavigationMenu]
ALTER TABLE [tNavigationMenu] DROP CONSTRAINT [CK_tNavigationMenu_Locale]
GO
ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD CONSTRAINT [CK_tNavigationMenu_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [CK_tNavigationMenu_Locale]
GO
--[tNavigationMenuItem]
ALTER TABLE [tNavigationMenuItem] DROP CONSTRAINT [CK_tNavigationMenuItem_Locale]
GO
ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD CONSTRAINT [CK_tNavigationMenuItem_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [CK_tNavigationMenuItem_Locale]
GO
--[tNews]
ALTER TABLE [tNews] DROP CONSTRAINT [CK_tNews_Locale]
GO
ALTER TABLE [tNews]  WITH CHECK 
	ADD CONSTRAINT [CK_tNews_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [CK_tNews_Locale]
GO
--[tPoll]
ALTER TABLE [tPoll] DROP CONSTRAINT [CK_tPoll_Locale]
GO
ALTER TABLE [tPoll]  WITH CHECK 
	ADD CONSTRAINT [CK_tPoll_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tPoll] CHECK CONSTRAINT [CK_tPoll_Locale]
GO
--[tNewsletter]
ALTER TABLE [tNewsletter] DROP CONSTRAINT [CK_tNewsletter_Locale]
GO
ALTER TABLE [tNewsletter]  WITH CHECK 
	ADD CONSTRAINT [CK_tNewsletter_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))	
GO
ALTER TABLE [tNewsletter] CHECK CONSTRAINT [CK_tNewsletter_Locale]
GO
--[tVocabulary]
ALTER TABLE [tVocabulary] DROP CONSTRAINT [CK_tVocabulary_Locale]
GO
ALTER TABLE [tVocabulary]  WITH CHECK 
	ADD CONSTRAINT [CK_tVocabulary_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tVocabulary] CHECK CONSTRAINT [CK_tVocabulary_Locale]
GO
--[tArticle]
ALTER TABLE [tArticle] DROP CONSTRAINT [CK_tArticle_Locale]
GO
ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [CK_tArticle_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [CK_tArticle_Locale]
GO
--[tBlog]
ALTER TABLE [tBlog] DROP CONSTRAINT [CK_tBlog_Locale]
GO
ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [CK_tBlog_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de' OR [Locale]='ru'))
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [CK_tBlog_Locale]
GO
