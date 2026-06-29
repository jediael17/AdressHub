# AddressHub

Aplicação web em **ASP.NET MVC (.NET 8)** que permite ao usuário realizar login e gerenciar um CRUD de endereços, com integração à API do **ViaCEP** e exportação para **CSV**.

---

## Funcionalidades

- **Autenticação** — Login com validação de credenciais e mensagem de erro em caso de credenciais incorretas.
- **CRUD de Endereços** — Adicionar, visualizar, editar e excluir endereços por usuário.
- **Modal de confirmação** — Ao excluir um endereço, um modal estilizado solicita confirmação antes de prosseguir.
- **Busca por CEP** — Preenchimento automático via integração com [ViaCEP](https://viacep.com.br/).
- **Exportação CSV** — Download dos endereços cadastrados em arquivo `.csv`.
- **Design responsivo e futurístico** — Interface dark com tema neon cyan.

---

## Tecnologias

| Camada    | Tecnologia                          |
|-----------|-------------------------------------|
| Backend   | ASP.NET MVC (.NET 8)                |
| ORM       | Entity Framework Core 8             |
| Banco     | PostgreSQL (Railway)                |
| Frontend  | HTML5 + CSS3 + JavaScript (Vanilla) |
| CSV       | CsvHelper                           |
| Auth      | Cookie Authentication (ASP.NET)     |
| CEP API   | ViaCEP (https://viacep.com.br)      |

---

## Estrutura do Projeto

```
AddressManager/
├── Controllers/
│   ├── AccountController.cs     # Login / Logout
│   └── AddressController.cs     # CRUD + exportação CSV + busca CEP
├── Data/
│   └── AppDbContext.cs           # DbContext EF Core (PostgreSQL)
├── Models/
│   └── Entities.cs               # Usuario, Endereco
├── Services/
│   └── Services.cs               # SecurityService, ViaCepService, CsvExportService
├── ViewModels/
│   └── ViewModels.cs             # LoginViewModel, EnderecoViewModel, ViaCepResponse
├── Views/
│   ├── Account/
│   │   └── Login.cshtml
│   ├── Address/
│   │   ├── Index.cshtml          # Listagem + modal de exclusão
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── _AddressForm.cshtml   # Formulário compartilhado com busca de CEP
│   └── Shared/
│       └── _Layout.cshtml        # Navbar + estrutura geral
├── wwwroot/
│   ├── css/app.css               # Tema futurístico dark
│   └── js/main.js                # CEP lookup, modal, toasts
├── Scripts/SQL/
│   ├── CreateTables.sql              # Script SQL Server (referência)
│   └── CreateTables_PostgreSQL.sql   # Script PostgreSQL
└── appsettings.json
```

---

## Banco de Dados

O projeto utiliza **PostgreSQL hospedado no Railway**. As tabelas são criadas automaticamente na primeira execução via `EnsureCreated()`.

### Tabela `usuarios`

| Coluna       | Tipo         | Descrição                   |
|--------------|--------------|-----------------------------|
| id           | SERIAL       | Chave primária              |
| nome         | VARCHAR(150) | Nome do usuário             |
| usuario      | VARCHAR(100) | Login único                 |
| senha        | VARCHAR(256) | Hash SHA-256 em Base64      |
| datacriacao  | TIMESTAMP    | Data de criação do registro |

### Tabela `enderecos`

| Coluna          | Tipo         | Descrição                         |
|-----------------|--------------|-----------------------------------|
| id              | SERIAL       | Chave primária                    |
| cep             | CHAR(8)      | CEP sem máscara                   |
| logradouro      | VARCHAR(200) | Rua / Avenida                     |
| complemento     | VARCHAR(100) | Complemento (opcional)            |
| bairro          | VARCHAR(100) | Bairro                            |
| cidade          | VARCHAR(100) | Cidade                            |
| uf              | CHAR(2)      | Estado (sigla)                    |
| numero          | VARCHAR(20)  | Número do imóvel                  |
| usuarioid       | INT          | FK → usuarios(id)                 |
| datacriacao     | TIMESTAMP    | Data de criação                   |
| dataatualizacao | TIMESTAMP    | Data da última atualização (null) |

---

## Segurança

- Senhas armazenadas como **hash SHA-256 em Base64**.
- Proteção contra **CSRF** com `AntiForgeryToken` em todos os formulários POST.
- Autenticação via **Cookie HttpOnly + SameSite Strict**.
- Isolamento de dados por usuário — cada usuário só acessa seus próprios endereços.

---

## Como rodar localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Uma instância PostgreSQL (local ou na nuvem)

### Passo a passo

**1. Clone o repositório**
```bash
git clone https://github.com/seu-usuario/AddressManager.git
cd AddressManager
```

**2. Configure o banco de dados**

Abra o arquivo `appsettings.json` e ajuste a connection string com seus dados:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=SEU_HOST;Port=5432;Database=SEU_BANCO;Username=SEU_USUARIO;Password=SUA_SENHA;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

> Se quiser usar PostgreSQL local sem SSL, use:
> `"Host=localhost;Port=5432;Database=addressmanager;Username=postgres;Password=SUA_SENHA"`

**3. Rode a aplicação**
```bash
dotnet restore
dotnet run
```

As tabelas e o usuário padrão são criados **automaticamente** na primeira execução.

**4. Acesse no navegador**
```
http://localhost:5000
```

**Credenciais padrão:**
| Campo   | Valor      |
|---------|------------|
| Usuário | `admin`    |
| Senha   | `Admin@123`|

---

## Commits por Funcionalidade

| Funcionalidade              | Commit                                  |
|-----------------------------|-----------------------------------------|
| Estrutura inicial           | `feat: initial project setup`           |
| SQL scripts                 | `feat: add SQL table creation scripts`  |
| Models e DbContext          | `feat: add entities and DbContext`      |
| Autenticação (login)        | `feat: implement cookie authentication` |
| CRUD de endereços           | `feat: implement address CRUD`          |
| Integração ViaCEP           | `feat: integrate ViaCEP API`            |
| Exportação CSV              | `feat: add CSV export`                  |
| Design futurístico          | `feat: futuristic dark UI`              |
| Migração para PostgreSQL    | `feat: migrate to PostgreSQL`           |
| Modal de confirmação        | `feat: add delete confirmation modal`   |

---
