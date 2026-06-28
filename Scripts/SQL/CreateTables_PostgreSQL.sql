-- ============================================================
-- Script PostgreSQL - AddressManager (Railway)
-- ============================================================

-- Tabela usuarios
CREATE TABLE IF NOT EXISTS usuarios (
    id          SERIAL          PRIMARY KEY,
    nome        VARCHAR(150)    NOT NULL,
    usuario     VARCHAR(100)    NOT NULL UNIQUE,
    senha       VARCHAR(256)    NOT NULL,
    datacriacao TIMESTAMP       NOT NULL DEFAULT NOW()
);

-- Tabela enderecos
CREATE TABLE IF NOT EXISTS enderecos (
    id              SERIAL          PRIMARY KEY,
    cep             CHAR(8)         NOT NULL,
    logradouro      VARCHAR(200)    NOT NULL,
    complemento     VARCHAR(100)    NULL,
    bairro          VARCHAR(100)    NOT NULL,
    cidade          VARCHAR(100)    NOT NULL,
    uf              CHAR(2)         NOT NULL,
    numero          VARCHAR(20)     NOT NULL,
    usuarioid       INT             NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    datacriacao     TIMESTAMP       NOT NULL DEFAULT NOW(),
    dataatualizacao TIMESTAMP       NULL
);

CREATE INDEX IF NOT EXISTS ix_enderecos_usuarioid ON enderecos(usuarioid);

-- Usuário padrão (senha: Admin@123)
INSERT INTO usuarios (nome, usuario, senha)
SELECT 'Administrador', 'admin', '0MeHDQBsHlQVFoMiHBOFaA0s3xSa1pM6qthQnbASWgk='
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'admin');
