CREATE PROCEDURE [dbo].[spUserLookup]
	@id nvarchar(128)
AS
begin
	set nocount on;

	select Id, FirstName, LastName, EmailAddress, CreatedDate
	from [dbo].[User]
	where Id = @id
end