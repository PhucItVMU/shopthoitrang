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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [Fullname] nvarchar(max) NULL,
        [Phone] nvarchar(max) NULL,
        [RoleNames] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Adv] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [Image] nvarchar(500) NOT NULL,
        [Link] nvarchar(500) NOT NULL,
        [Type] int NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Adv] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Category] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Alias] nvarchar(max) NOT NULL,
        [Link] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [SeoTitle] nvarchar(150) NOT NULL,
        [SeoDescription] nvarchar(250) NOT NULL,
        [SeoKeywords] nvarchar(150) NOT NULL,
        [IsActive] bit NOT NULL,
        [Position] int NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Category] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Contact] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Website] nvarchar(max) NOT NULL,
        [Message] nvarchar(4000) NOT NULL,
        [IsRead] nvarchar(max) NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Contact] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Order] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(max) NOT NULL,
        [CustomerName] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [TypePayment] int NOT NULL,
        [Status] int NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Order] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_ProductCategory] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Alias] nvarchar(150) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Icon] nvarchar(250) NULL,
        [SeoTitle] nvarchar(250) NULL,
        [SeoDescription] nvarchar(500) NULL,
        [SeoKeywords] nvarchar(250) NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_ProductCategory] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Subscribe] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(max) NOT NULL,
        [CreateDate] datetime2 NOT NULL,
        CONSTRAINT [PK_tb_Subscribe] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_SystemSettin] (
        [SettingKey] nvarchar(50) NOT NULL,
        [SettingValue] nvarchar(4000) NOT NULL,
        [SettingDescription] nvarchar(4000) NOT NULL,
        CONSTRAINT [PK_tb_SystemSettin] PRIMARY KEY ([SettingKey])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [ThongKes] (
        [Id] int NOT NULL IDENTITY,
        [ThoiGian] datetime2 NOT NULL,
        [SoTruyCap] bigint NOT NULL,
        CONSTRAINT [PK_ThongKes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_News] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Alias] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Detail] nvarchar(max) NOT NULL,
        [Image] nvarchar(max) NOT NULL,
        [CategoryId] int NOT NULL,
        [SeoTitle] nvarchar(max) NOT NULL,
        [SeoDescription] nvarchar(max) NOT NULL,
        [SeoKeywords] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_News] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_tb_News_tb_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [tb_Category] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Posts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Alias] nvarchar(150) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Detail] nvarchar(max) NOT NULL,
        [Image] nvarchar(250) NOT NULL,
        [CategoryId] int NOT NULL,
        [SeoTitle] nvarchar(250) NOT NULL,
        [SeoDescription] nvarchar(500) NOT NULL,
        [SeoKeywords] nvarchar(200) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Posts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_tb_Posts_tb_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [tb_Category] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_Product] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(250) NOT NULL,
        [Alias] nvarchar(250) NULL,
        [ProductCode] nvarchar(50) NULL,
        [Description] nvarchar(max) NULL,
        [Detail] nvarchar(max) NULL,
        [Image] nvarchar(250) NULL,
        [OriginalPrice] decimal(18,2) NULL,
        [Price] decimal(18,2) NOT NULL,
        [PriceSale] decimal(18,2) NULL,
        [Quantity] int NULL,
        [ViewCount] int NULL,
        [IsHome] bit NOT NULL,
        [IsSale] bit NOT NULL,
        [IsFeature] bit NOT NULL,
        [IsHot] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [ProductCategoryId] int NULL,
        [SeoTitle] nvarchar(250) NULL,
        [SeoDescription] nvarchar(500) NULL,
        [SeoKeywords] nvarchar(250) NULL,
        [CreateBy] nvarchar(max) NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_tb_Product] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_tb_Product_tb_ProductCategory_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [tb_ProductCategory] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_OrderDetail] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [ProductId] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        CONSTRAINT [PK_tb_OrderDetail] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_tb_OrderDetail_tb_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [tb_Order] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_tb_OrderDetail_tb_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tb_Product] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE TABLE [tb_ProductImage] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [Image] nvarchar(max) NULL,
        [IsDefault] bit NOT NULL,
        CONSTRAINT [PK_tb_ProductImage] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_tb_ProductImage_tb_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tb_Product] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_News_CategoryId] ON [tb_News] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_OrderDetail_OrderId] ON [tb_OrderDetail] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_OrderDetail_ProductId] ON [tb_OrderDetail] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_Posts_CategoryId] ON [tb_Posts] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_Product_ProductCategoryId] ON [tb_Product] ([ProductCategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    CREATE INDEX [IX_tb_ProductImage_ProductId] ON [tb_ProductImage] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240512132142_initDatabase'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240512132142_initDatabase', N'8.0.4');
END;
GO

COMMIT;
GO

