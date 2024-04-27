CREATE PROCEDURE GetEmail
    @ClientUserId INT,
    @ClientUserEmail NVARCHAR(35) OUTPUT
AS
BEGIN
    SET @ClientUserEmail = (SELECT Email FROM ClientUsers WHERE Id = @ClientUserId)
END
