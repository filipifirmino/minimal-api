# Minimal API - Sistema de Gestão

API REST construída com .NET 9 Minimal APIs para gerenciamento de usuários e veículos, desenvolvida como parte do bootcamp da DIO (Digital Innovation One).

## 📋 Sobre o Projeto

Este projeto implementa uma API moderna utilizando o padrão Minimal API do .NET 9, oferecendo endpoints para gerenciamento de usuários e veículos com autenticação JWT, persistência em SQL Server via Entity Framework Core, e documentação interativa com Swagger.

## 🚀 Tecnologias Utilizadas

- **.NET 9.0** - Framework principal
- **ASP.NET Core Minimal APIs** - Arquitetura de endpoints
- **Entity Framework Core 9.0** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional
- **Swagger/OpenAPI** - Documentação interativa da API
- **BCrypt** - Hash de senhas (via PasswordHasher personalizado)
- **JWT** - Autenticação e autorização (configurado)

## 🏗️ Arquitetura do Projeto

O projeto segue uma arquitetura em camadas com separação de responsabilidades:

```
minimal-api/
├── Dominio/                    # Camada de domínio (regras de negócio)
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Entities/               # Entidades de domínio
│   ├── Enums/                  # Enumerações
│   ├── Mappers/                # Mapeadores entre camadas
│   ├── Services/               # Serviços de domínio
│   └── UseCases/               # Casos de uso (lógica de aplicação)
├── Endpoints/                  # Definição dos endpoints da API
├── Infra/                      # Camada de infraestrutura
│   ├── Context/                # Contexto do EF Core
│   ├── Entities/               # Entidades de persistência
│   └── Repository/             # Repositórios de dados
└── Migrations/                 # Migrações do banco de dados
```

### Principais Componentes

- **Entities**: Representam as entidades de domínio (`User`, `Vehicle`)
- **DTOs**: Objetos para transferência de dados entre camadas
- **UseCases**: Implementam os casos de uso da aplicação (ex: `CreateUserProcess`, `LoginProcess`)
- **Repositories**: Padrão Repository para acesso a dados
- **Mappers**: Conversão entre entidades de domínio e DTOs/Entities

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (ou SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Rider](https://www.jetbrains.com/rider/) (opcional)

## 🔧 Instalação e Configuração

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd minimal-api
```

### 2. Configure a Connection String

Edite o arquivo `src/minimal-api/appsettings.json` e ajuste a connection string para seu ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433; Database=MinimalDataBase; User Id=sa;Password=SUA_SENHA; TrustServerCertificate=true;MultipleActiveResultSets=true;Connection Timeout=60;Command Timeout=60;Integrated Security=false"
  }
}
```

### 3. Execute as Migrations

Navegue até o diretório do projeto e execute:

```bash
cd src/minimal-api
dotnet ef database update
```

Isso criará o banco de dados `MinimalDataBase` com as tabelas necessárias.

### 4. Execute a aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:7XXX` (porta definida no launchSettings.json)
- **HTTP**: `http://localhost:5XXX`

### 5. Acesse a documentação Swagger

Abra seu navegador e acesse:
```
https://localhost:7XXX/swagger
```

## 📚 Endpoints da API

### 🔐 Authentication

#### POST `/api/auth/login`
Realiza autenticação e retorna token JWT.

**Request Body:**
```json
{
  "email": "usuario@exemplo.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 60
}
```

---

### 👥 Users

#### POST `/api/users`
Cria um novo usuário no sistema.

**Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao@exemplo.com",
  "password": "senha123"
}
```

**Response (201 Created):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva",
  "email": "joao@exemplo.com",
  "status": "Active"
}
```

#### GET `/api/users`
Lista todos os usuários cadastrados.

**Response (200 OK):**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "João Silva",
    "email": "joao@exemplo.com",
    "status": "Active"
  }
]
```

#### GET `/api/users/{id}`
Busca um usuário específico por ID.

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva",
  "email": "joao@exemplo.com",
  "status": "Active"
}
```

#### PUT `/api/users/{id}`
Atualiza os dados de um usuário (exceto senha).

**Request Body:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva Santos",
  "email": "joao.santos@exemplo.com",
  "status": "Active"
}
```

#### DELETE `/api/users/{id}`
Remove um usuário do sistema.

**Response (204 No Content)**

---

### 🚗 Vehicles

#### GET `/api/vehicles`
Lista todos os veículos cadastrados.

**Response (200 OK):**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "brand": "Toyota",
    "model": "Corolla",
    "year": "2024",
    "color": "Preto",
    "licensePlate": "ABC-1234"
  }
]
```

#### GET `/api/vehicles/{id}`
Busca um veículo específico por ID.

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "brand": "Toyota",
  "model": "Corolla",
  "year": "2024",
  "color": "Preto",
  "licensePlate": "ABC-1234"
}
```

#### POST `/api/vehicles`
Cadastra um novo veículo.

**Request Body:**
```json
{
  "brand": "Toyota",
  "model": "Corolla",
  "year": "2024",
  "color": "Preto",
  "licensePlate": "ABC-1234"
}
```

**Response (201 Created):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "brand": "Toyota",
  "model": "Corolla",
  "year": "2024",
  "color": "Preto",
  "licensePlate": "ABC-1234"
}
```

#### DELETE `/api/vehicles/{id}` 🔒
Remove um veículo do sistema (requer autenticação).

**Response (204 No Content)**

> **Nota:** Este endpoint requer token JWT no header: `Authorization: Bearer {token}`

## 🗄️ Banco de Dados

### Modelo de Dados

#### Tabela: Users
| Coluna   | Tipo         | Descrição                    |
|----------|--------------|------------------------------|
| Id       | UNIQUEIDENTIFIER | Chave primária (GUID)   |
| Name     | VARCHAR(100) | Nome do usuário              |
| Email    | VARCHAR(100) | Email (único)                |
| Password | VARCHAR(255) | Senha hasheada               |
| Status   | INT          | Status (0=Active, 1=Inactive)|

#### Tabela: Vehicles
| Coluna       | Tipo         | Descrição                |
|--------------|--------------|--------------------------|
| Id           | UNIQUEIDENTIFIER | Chave primária (GUID)|
| Brand        | VARCHAR(100) | Marca do veículo         |
| Model        | VARCHAR(100) | Modelo do veículo        |
| Year         | VARCHAR(10)  | Ano de fabricação        |
| Color        | VARCHAR(50)  | Cor do veículo           |
| LicensePlate | VARCHAR(20)  | Placa                    |

### Migrations

O projeto possui as seguintes migrations:

1. **20251010191821_initialCreate** - Criação inicial das tabelas
2. **20251013195839_UpdatedDataContext** - Atualização do contexto de dados

Para criar uma nova migration:
```bash
dotnet ef migrations add NomeDaMigration
```

Para aplicar migrations pendentes:
```bash
dotnet ef database update
```

Para reverter para uma migration específica:
```bash
dotnet ef database update NomeDaMigration
```

## 🔐 Segurança

### Hash de Senhas
As senhas são hasheadas utilizando BCrypt através do serviço `PasswordHasher` antes de serem armazenadas no banco de dados.

### JWT (Configurado)
O projeto está configurado para trabalhar com JWT (JSON Web Tokens) para autenticação. As configurações estão em `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "sua-chave-secreta",
    "Issuer": "MinimalApi",
    "Audience": "MinimalApiClients",
    "ExpirationMinutes": 60
  }
}
```

> **⚠️ Importante:** Em produção, utilize variáveis de ambiente para armazenar credenciais sensíveis.

## 🧪 Testando a API

### Usando Swagger UI
1. Execute a aplicação
2. Acesse `https://localhost:7XXX/swagger`
3. Teste os endpoints diretamente pela interface

### Usando arquivos HTTP
O projeto inclui arquivos `.http` para testes:
- `minimal-api-endpoints.http`
- `minimal-api.http`

Utilize extensões como REST Client (VS Code) ou HTTP Client (Rider) para executá-los.

### Exemplo com cURL

**Criar usuário:**
```bash
curl -X POST https://localhost:7XXX/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@exemplo.com",
    "password": "senha123"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:7XXX/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@exemplo.com",
    "password": "senha123"
  }'
```

## 🎯 Funcionalidades Implementadas

- ✅ CRUD completo de usuários
- ✅ CRUD completo de veículos
- ✅ Autenticação com hash de senha (BCrypt)
- ✅ Login com validação de credenciais
- ✅ **Validação de modelos com FluentValidation**
- ✅ **Paginação nos endpoints de listagem**
- ✅ Padrão Repository
- ✅ DTOs para transferência de dados
- ✅ Mappers para conversão entre camadas
- ✅ UseCases para lógica de negócio
- ✅ Validação de unicidade de email
- ✅ Documentação Swagger/OpenAPI
- ✅ Entity Framework Core com SQL Server
- ✅ Migrations para versionamento do banco
- ✅ Tratamento de erros
- ✅ Separação em camadas (Domain, Infrastructure)
- ✅ Testes unitários com xUnit

## ✨ Recursos Adicionais Implementados

### Validação com FluentValidation

O projeto utiliza FluentValidation para validação robusta de modelos:

- **CreateUserRequestValidator**: Valida criação de usuários (nome, email, senha)
- **UpdateUserRequestValidator**: Valida atualização de usuários
- **CreateVehicleRequestValidator**: Valida criação de veículos (inclui validação de placas no formato brasileiro e Mercosul)
- **LoginRequestValidator**: Valida credenciais de login

As validações retornam erros estruturados com mensagens em português.

### Paginação

Os endpoints de listagem (`GET /api/users` e `GET /api/vehicles`) suportam paginação opcional:

```bash
GET /api/users?pageNumber=1&pageSize=10
```

**Resposta paginada:**
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 100,
  "totalPages": 10,
  "hasPrevious": false,
  "hasNext": true
}
```

### Testes Unitários

O projeto inclui 34 testes unitários cobrindo:
- **Validators**: Testes de todas as regras de validação
- **UseCases**: Testes de lógica de negócio (CreateUser, Login)
- **Repositories**: Testes de operações de dados incluindo paginação

Execute os testes com:
```bash
dotnet test
```

## 🔄 Melhorias Futuras

- [ ] Implementar refresh tokens
- [ ] Adicionar testes de integração
- [ ] Adicionar filtros e ordenação avançados
- [ ] Implementar rate limiting
- [ ] Adicionar logging estruturado (Serilog)
- [ ] Implementar cache (Redis)
- [ ] Implementar CORS configurável
- [ ] Adicionar health checks
- [ ] Containerização com Docker
- [ ] CI/CD pipeline

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.


---

⭐ Se este projeto foi útil para você, considere dar uma estrela!
