IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [ClientUsers] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(40) NOT NULL,
        [CardNumber] nvarchar(16) NOT NULL,
        [Status] bit NOT NULL,
        [Email] nvarchar(35) NOT NULL,
        [PasswordHash] varbinary(max) NOT NULL,
        [SuscriptionDate] datetime2 NOT NULL,
        [SubscriptionId] int NOT NULL,
        CONSTRAINT [PK_ClientUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [Countries] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [Genres] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [Movies] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(60) NOT NULL,
        [Duration] decimal(10,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [PremiereDate] datetime2 NOT NULL,
        [Rate] decimal(3,2) NOT NULL,
        [FirebasePosterId] nvarchar(450) NOT NULL,
        [Price] decimal(4,2) NOT NULL,
        [IsFree] bit NOT NULL,
        CONSTRAINT [PK_Movies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [PreRentItems] (
        [Id] int NOT NULL IDENTITY,
        [MovieId] int NOT NULL,
        [RentId] uniqueidentifier NOT NULL,
        [RentDuration] int NOT NULL,
        CONSTRAINT [PK_PreRentItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [PreRents] (
        [Id] uniqueidentifier NOT NULL,
        [Date] datetime2 NOT NULL,
        [ClientUserId] int NOT NULL,
        CONSTRAINT [PK_PreRents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [RentItems] (
        [Id] int NOT NULL IDENTITY,
        [MovieId] int NOT NULL,
        [RentId] uniqueidentifier NOT NULL,
        [RentDuration] int NOT NULL,
        CONSTRAINT [PK_RentItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [Rents] (
        [Id] uniqueidentifier NOT NULL,
        [RentDate] datetime2 NOT NULL,
        [ClientUserId] int NOT NULL,
        CONSTRAINT [PK_Rents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE TABLE [Subscriptions] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(20) NOT NULL,
        [Price] decimal(5,2) NOT NULL,
        [Features] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientUsers_Email] ON [ClientUsers] ([Email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE UNIQUE INDEX [IX_Countries_Name] ON [Countries] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE UNIQUE INDEX [IX_Genres_Name] ON [Genres] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE UNIQUE INDEX [IX_Movies_FirebasePosterId] ON [Movies] ([FirebasePosterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    CREATE UNIQUE INDEX [IX_Subscriptions_Name] ON [Subscriptions] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240426093600_Reset')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240426093600_Reset', N'6.0.29');
END;
GO

COMMIT;
GO

