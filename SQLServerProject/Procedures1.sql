CREATE PROCEDURE dbo.AddSubscription
    @Name NVARCHAR(20),
    @Price DECIMAL(5,2),
    @Features NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO dbo.Subscription (Name, Price, Features)
    VALUES (@Name, @Price, @Features)
END

CREATE PROCEDURE dbo.AddCountry
	@Name NVARCHAR(30)
AS
BEGIN
    INSERT INTO dbo.Country (Name)
    VALUES (@Name)
END

CREATE PROCEDURE dbo.AddUserClient
    @Username NVARCHAR(40),
    @CardNumber NVARCHAR(16),
    @Status BIT,
    @Email NVARCHAR(35),
    @PasswordHash VARBINARY(64),
    @SubscriptionDate DATE,
    @SubscriptionId INT,
    @CountryId INT
AS
BEGIN
    INSERT INTO dbo.UserClient (Username, CardNumber, Status, Email, PasswordHash, SubscriptionDate, SubscriptionId, CountryId)
    VALUES (@Username, @CardNumber, @Status, @Email, @PasswordHash, @SubscriptionDate, @SubscriptionId, @CountryId)
END