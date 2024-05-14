--TESTING

CREATE PROCEDURE [dbo].[GetEmail]
    @ClientUserId INT
--	@ClientUserEmail NVARCHAR(35) OUTPUT
AS
BEGIN
    SELECT Email FROM ClientUsers c WHERE c.Id = @ClientUserId
END

Create procedure [dbo].[GetAllMovies]
as
begin
	select * from Movies
end