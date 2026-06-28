-- ============================================================
-- Script de Criação das Tabelas - AddressManager
-- Banco de Dados: SQL Server
-- ============================================================

USE master;
GO

-- Cria o banco de dados caso não exista
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AddressManagerDB')
BEGIN
    CREATE DATABASE AddressManagerDB;
END
GO

USE AddressManagerDB;
GO

-- ============================================================
-- Tabela: Usuarios
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id          INT             NOT NULL IDENTITY(1,1),
        Nome        NVARCHAR(150)   NOT NULL,
        Usuario     NVARCHAR(100)   NOT NULL,
        Senha       NVARCHAR(256)   NOT NULL,   -- Hash SHA-256 em Base64
        DataCriacao DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
        CONSTRAINT UQ_Usuarios_Usuario UNIQUE (Usuario)
    );
END
GO

-- ============================================================
-- Tabela: Enderecos
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Enderecos')
BEGIN
    CREATE TABLE Enderecos (
        Id           INT            NOT NULL IDENTITY(1,1),
        Cep          CHAR(8)        NOT NULL,
        Logradouro   NVARCHAR(200)  NOT NULL,
        Complemento  NVARCHAR(100)  NULL,
        Bairro       NVARCHAR(100)  NOT NULL,
        Cidade       NVARCHAR(100)  NOT NULL,
        Uf           CHAR(2)        NOT NULL,
        Numero       NVARCHAR(20)   NOT NULL,
        UsuarioId    INT            NOT NULL,
        DataCriacao  DATETIME2      NOT NULL DEFAULT GETDATE(),
        DataAtualizacao DATETIME2   NULL,

        CONSTRAINT PK_Enderecos PRIMARY KEY (Id),
        CONSTRAINT FK_Enderecos_Usuarios
            FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
            ON DELETE CASCADE
    );
END
GO

-- Índice para busca por usuário
CREATE INDEX IX_Enderecos_UsuarioId ON Enderecos(UsuarioId);
GO

-- ============================================================
-- Usuário padrão para testes (senha: Admin@123)
-- Hash SHA-256 de "Admin@123"
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Usuario = 'admin')
BEGIN
    INSERT INTO Usuarios (Nome, Usuario, Senha)
    VALUES (
        'Administrador',
        'admin',
        'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg='  -- SHA-256 Base64 de "Admin@123"
    );
END
GO

PRINT 'Tabelas criadas com sucesso!';
GO
