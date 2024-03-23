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

CREATE TABLE dbo.User_Client (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(40) NOT NULL,
    Card_Number NVARCHAR(16),
    Status BIT NOT NULL,
    Email NVARCHAR(35) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64),
    Subscription_Date DATE NOT NULL,
    Id_Subscription INT NOT NULL,
    Id_Country INT NOT NULL,
    CONSTRAINT FK_UserClient_Subscription FOREIGN KEY (Id_Subscription) REFERENCES dbo.Subscription (Id),
    CONSTRAINT FK_UserClient_Country FOREIGN KEY (Id_Country) REFERENCES dbo.Country (Id)
);