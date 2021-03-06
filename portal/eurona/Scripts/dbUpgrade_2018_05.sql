
delete from tReklamniZasilkySouhlas where Id_zasilky=3
delete from tReklamniZasilky where Id_zasilky=3



DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT
SELECT @MasterPageId = MasterPageId FROM tMasterPage WHERE Name='Default'
---------------------------------------------------------------------------------------------------------
-- Anonymous Registration content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content SK</p>', 'sk', 'anonymous-registration-content', 'Anonymous Registration content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content CS</p>', 'cs', 'anonymous-registration-content', 'Anonymous Registration content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content EN</p>', 'en', 'anonymous-registration-content', 'Anonymous Registration content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content PL</p>', 'pl', 'anonymous-registration-content', 'Anonymous Registration content', GETDATE(), 'C', 1)

---------------------------------------------------------------------------------------------------------
-- Order bottom content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content SK</p>', 'sk', 'order-bottom-info-content', 'Order bottom info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content CS</p>', 'cs', 'order-bottom-info-content', 'Order bottom info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content EN</p>', 'en', 'order-bottom-info-content', 'Order bottom info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content PL</p>', 'pl', 'order-bottom-info-content', 'Order bottom info content', GETDATE(), 'C', 1)

---------------------------------------------------------------------------------------------------------
-- Advisor detail top content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content SK</p>', 'sk', 'advisor-detail-top-info-content', 'Advisor detail top info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content CS</p>', 'cs', 'advisor-detail-top-info-content', 'Advisor detail top info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content EN</p>', 'en', 'advisor-detail-top-info-content', 'Advisor detail top info content', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @MasterPageId, '<p style="text-align: center;">Editovatelny content PL</p>', 'pl', 'advisor-detail-top-info-content', 'Advisor detail top info content', GETDATE(), 'C', 1)