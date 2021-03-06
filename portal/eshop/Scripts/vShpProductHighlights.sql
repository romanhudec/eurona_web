ALTER VIEW vShpProductHighlights
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ph.ProductHighlightsId, ph.InstanceId, ph.ProductId, ph.HighlightId,
	h.Name, h.Code, h.Icon, h.Notes
FROM
	tShpProductHighlights ph
	INNER JOIN vShpHighlights h ON h.HighlightId = ph.HighlightId
GO
