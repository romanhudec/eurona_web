ALTER TRIGGER rOrganizationUpdated
	ON  tOrganization
	--%%WITH ENCRYPTION%%
	AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	--WAITFOR DELAY '00:00:05';
	
	-- Vrati posledny (prave insertuty) prihoz
	DECLARE @OrganizationId INT, @AccountId INT, @Email NVARCHAR(100)

	SELECT @OrganizationId=OrganizationId, @AccountId=AccountId, @Email=ContactEmail FROM INSERTED
	IF @Email IS NOT NULL AND LEN(@Email) > 0 
	UPDATE tAccount SET Email=@Email WHERE AccountId=@AccountId
END
GO
