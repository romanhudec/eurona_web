------------------------------------------------------------------------------------------------------------------------

UPDATE tUrlAlias SET Alias='~/eshop/vanoce2014!', Name='Vanoce 2014'  WHERE Alias='~/eshop/novinky'
GO
UPDATE tPage SET Name='Vanoce 2014!' WHERE Name = 'Novinky!'
GO
UPDATE tNavigationMenu SET Name='Vanoce 2014!' WHERE Name = 'Novinky!'
GO
UPDATE tUrlAlias SET Url='~/eshop/catalog_vanoce.aspx' WHERE Alias='~/eshop/vanoce2014!'
GO
------------------------------------------------------------------------------------------------------------------------
