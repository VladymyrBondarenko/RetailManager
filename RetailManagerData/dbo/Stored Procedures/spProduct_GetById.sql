CREATE PROCEDURE [dbo].[spProduct_GetById]
	@id int
AS
begin
	select 
		Product.[Id],
		Product.[ProductName],
		Product.[Description],
		Product.[RetailPrice],
		Product.[QuantityInStock],
		Product.[IsTaxable]
	from Product 
	where Id = @id
end