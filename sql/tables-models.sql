IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Fornecedores] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] nvarchar(200) NOT NULL,
    [Documento] nvarchar(14) NOT NULL,
    [TipoFornecedor] int NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Fornecedores] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Enderecos] (
    [Id] uniqueidentifier NOT NULL,
    [FornecedorId] uniqueidentifier NOT NULL,
    [Logradouro] nvarchar(200) NOT NULL,
    [Numero] nvarchar(50) NOT NULL,
    [Complemento] nvarchar(250) NOT NULL,
    [Cep] nvarchar(8) NOT NULL,
    [Bairro] nvarchar(100) NOT NULL,
    [Cidade] nvarchar(100) NOT NULL,
    [Estado] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Enderecos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Enderecos_Fornecedores_FornecedorId] FOREIGN KEY ([FornecedorId]) REFERENCES [Fornecedores] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Produtos] (
    [Id] uniqueidentifier NOT NULL,
    [FornecedorId] uniqueidentifier NOT NULL,
    [Nome] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(1000) NOT NULL,
    [Imagem] nvarchar(100) NOT NULL,
    [Valor] money NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Produtos_Fornecedores_FornecedorId] FOREIGN KEY ([FornecedorId]) REFERENCES [Fornecedores] ([Id]) ON DELETE CASCADE
);

GO

CREATE UNIQUE INDEX [IX_Enderecos_FornecedorId] ON [Enderecos] ([FornecedorId]);

GO

CREATE INDEX [IX_Produtos_FornecedorId] ON [Produtos] ([FornecedorId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210106030114_Initial', N'3.1.10');

GO

