------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.1 to 0.2
------------------------------------------------------------------------------------------------------------------------
-- Upgrade

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary

CREATE TABLE [dbo].[tVocabulary](
	[VocabularyId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Notes] [nvarchar](200) NULL,
 CONSTRAINT [PK_VocabularyId] PRIMARY KEY CLUSTERED ([VocabularyId] ASC)
)
GO

ALTER TABLE [tVocabulary]  WITH CHECK 
	ADD CONSTRAINT [CK_tVocabulary_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tVocabulary] CHECK CONSTRAINT [CK_tVocabulary_Locale]
GO

------------------------------------------------------------------------------------------------------------------------
-- Translation

CREATE TABLE [dbo].[tTranslation](
	[TranslationId] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyId] [int] NOT NULL,
	[Term] [nvarchar](500) NOT NULL,
	[Translation] [nvarchar](4000) NOT NULL,
	[Notes] [nvarchar](4000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_TranslationId] PRIMARY KEY CLUSTERED ([TranslationId] ASC)
)
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_Vocabulary] FOREIGN KEY([VocabularyId])
	REFERENCES [tVocabulary] ([VocabularyId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_Vocabulary]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tTranslation] ([TranslationId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_HistoryId]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [CK_tTranslation_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [CK_tTranslation_HistoryType]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & translation

CREATE VIEW vVocabularies AS SELECT A=1
GO

ALTER VIEW vVocabularies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[VocabularyId], [Locale], [Name], [Notes]
FROM tVocabulary
GO

CREATE VIEW vTranslations AS SELECT A=1
GO

ALTER VIEW vTranslations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.VocabularyId, VocabularyName = v.Name, v.Locale, t.TranslationId, t.Term, t.Translation, t.Notes
FROM tTranslation t (NOLOCK)
INNER JOIN tVocabulary v (NOLOCK) ON t.VocabularyId = v.VocabularyId
WHERE t.HistoryId IS NULL
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & Translation

-- iba tato procka je urcena pre UI. User si moze len upravit text
CREATE PROCEDURE pTranslationModify AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pTranslationModify
	@HistoryAccount INT,
	@TranslationId INT,
	@Translation NVARCHAR(4000) = NULL, 
	@Notes NVARCHAR(4000), 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTranslation WHERE TranslationId = @TranslationId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @Translation %d', 16, 1, @Translation);
		RETURN;
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTranslation (VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @TranslationId
		FROM tTranslation
		WHERE Translation = @Translation

		UPDATE tTranslation
		SET
			Translation = @Translation, Notes = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TranslationId = @TranslationId

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

-- developerska procka pre zalozenie noveho textu v scripte
CREATE PROCEDURE pTranslationCreateEx AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pTranslationCreateEx
	@HistoryAccount INT,
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
			INSERT INTO tVocabulary(Locale, Name, Notes) VALUES (@Locale, @Vocabulary, '')
			SET @VocabularyId = SCOPE_IDENTITY()
		END

		DECLARE @TranslationId INT
		SELECT @TranslationId = TranslationId FROM vTranslations WHERE VocabularyId = @VocabularyId AND Term = @Term		
		IF @TranslationId IS NULL BEGIN
			INSERT INTO tTranslation(VocabularyId, Term, Translation, Notes,
				HistoryStamp, HistoryType, HistoryAccount, HistoryId) 
			VALUES (@VocabularyId, @Term, @Translation, @Notes,
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

