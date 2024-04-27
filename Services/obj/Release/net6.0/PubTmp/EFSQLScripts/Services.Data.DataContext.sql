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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
BEGIN
    CREATE TABLE [Countries] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientUsers_Email] ON [ClientUsers] ([Email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
BEGIN
    CREATE UNIQUE INDEX [IX_Countries_Name] ON [Countries] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
BEGIN
    CREATE UNIQUE INDEX [IX_Subscriptions_Name] ON [Subscriptions] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240405181301_Second')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240405181301_Second', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406050207_Third')
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
        CONSTRAINT [PK_Movies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406050207_Third')
BEGIN
    CREATE UNIQUE INDEX [IX_Movies_FirebasePosterId] ON [Movies] ([FirebasePosterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406050207_Third')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240406050207_Third', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406050341_Fourth')
BEGIN
    ALTER TABLE [Movies] ADD [IsFree] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406050341_Fourth')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240406050341_Fourth', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406103600_Five')
BEGIN
    CREATE TABLE [Rents] (
        [Id] int NOT NULL IDENTITY,
        [RentDate] datetime2 NOT NULL,
        [ClientUserId] int NOT NULL,
        [MovieId] int NOT NULL,
        CONSTRAINT [PK_Rents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240406103600_Five')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240406103600_Five', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413083827_Genre')
BEGIN
    CREATE TABLE [Genres] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413083827_Genre')
BEGIN
    CREATE UNIQUE INDEX [IX_Genres_Name] ON [Genres] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413083827_Genre')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240413083827_Genre', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413093349_rent-updated')
BEGIN
    ALTER TABLE [Rents] ADD [RentDuration] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413093349_rent-updated')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240413093349_rent-updated', N'6.0.29');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413095448_rents')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Rents]') AND [c].[name] = N'MovieId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Rents] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Rents] DROP COLUMN [MovieId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413095448_rents')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Rents]') AND [c].[name] = N'RentDuration');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Rents] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Rents] DROP COLUMN [RentDuration];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413095448_rents')
BEGIN
    EXEC sp_rename N'[Rents].[RentDate]', N'BuyDate', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413095448_rents')
BEGIN
    CREATE TABLE [RentItems] (
        [Id] int NOT NULL IDENTITY,
        [MovieId] int NOT NULL,
        [RentId] int NOT NULL,
        [RentDuration] int NOT NULL,
        CONSTRAINT [PK_RentItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240413095448_rents')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240413095448_rents', N'6.0.29');
END;
GO

COMMIT;
GO

