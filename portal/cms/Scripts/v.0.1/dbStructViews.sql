------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- classifiers
CREATE VIEW vAddresses AS SELECT A=1
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
-- EOF Views declarations
------------------------------------------------------------------------------------------------------------------------
