CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin
	set nocount on;

	select 
		Product.[Id],
		Product.[ProductName],
		Product.[Description],
		Product.[RetailPrice],
		Product.[QuantityInStock],
		Product.[IsTaxable]
	from dbo.Product
	order by ProductName
end