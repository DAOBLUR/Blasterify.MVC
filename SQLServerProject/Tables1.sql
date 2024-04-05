CREATE TABLE dbo.Subscription (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(20) NOT NULL,
    Price DECIMAL(5,2) NOT NULL,
    Features NVARCHAR(MAX) NOT NULL
);

CREATE TABLE dbo.Country (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(30) NOT NULL
);

CREATE TABLE dbo.UserClient (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(40) NOT NULL,
    CardNumber NVARCHAR(16),
    Status BIT NOT NULL,
    Email NVARCHAR(35) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64),
    SubscriptionDate DATE NOT NULL,
    SubscriptionId INT NOT NULL,
    CountryId INT NOT NULL,
    CONSTRAINT FK_UserClient_Subscription FOREIGN KEY (SubscriptionId) REFERENCES dbo.Subscription (Id),
    CONSTRAINT FK_UserClient_Country FOREIGN KEY (CountryId) REFERENCES dbo.Country (Id)
);

CREATE TABLE CharacterActor (
    CharacterId INT NOT NULL,
    ActorId INT NOT NULL,
    FOREIGN KEY (CharacterId) REFERENCES Character (Id),
    FOREIGN KEY (ActorId) REFERENCES Actor (Id),
    PRIMARY KEY (CharacterId, ActorId)
);
