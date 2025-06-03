# FCG.FiapCloudGames
# 🎮 Fiap Cloud Games API

API desenvolvida para gerenciamento de usuários e jogos, com foco em boas práticas de arquitetura DDD, autenticação segura, validação robusta e testes automatizados.

---

## 📌 Objetivo

Desenvolver uma API RESTful robusta e escalável, aplicando:

- Domain-Driven Design (DDD) 
- Clean Architecture 
- Principios SOLID 
- Middleware para log de requisições e traces 
- Middleware de tratamento de erros centralizado
- Exceptions personalizadas
- Uso do Entity Framework Core com migrations
- Autenticação baseada em JWT
- Autorização baseada em permissões
- Hash seguro de senhas com salt
- Validações de domínio e DTOs
- Testes unitários com TDD 
- Documententação Swagger com Swashbuckle.AspNetCore

---

## 🚀 Tecnologias Utilizadas

| Tecnologia        | Versão/Detalhes                  |
|-------------------|----------------------------------|
| .NET              | .NET 8                           |
| C#                | 12                               |
| Entity Framework  | Core, com Migrations             |
| Banco de Dados    | SQL Server (ou SQLite para testes) |
| Autenticação      | JWT (Bearer Token)               |
| Testes            | xUnit, Moq, FluentAssertions     |
| Swagger           | Swashbuckle.AspNetCore           |
| Segurança         | PBKDF2 + salt com SHA256         |
| Logger            | Middleware de Request/Response + LogId |

---

## 🧠 Padrões e Boas Práticas

- Camadas separadas por responsabilidade (Domain, Application, Infrastructure, API)
- Interfaces para abstração de serviços externos no domínio
- Injeção de dependência configurada via Program.cs
- Tratamento global de exceções via middleware
- DTOs com validações automáticas via DataAnnotations

---

## ✅ Principais Funcionalidades

### Usuários
- ✅ Criação de usuários
- ✅ Autenticação com JWT
- ✅ Hash seguro de senhas com salt
- ✅ Verificação de senha no login
- ✅ Validação de senha forte
- ✅ Validação de formato de e-mail
- ✅ Controle de permissões (admin)

### Jogos
- ✅ Cadastro e listagem de jogos
- ✅ Validação de campos e tamanho máximo
- ✅ Validação de gênero permitido
- ✅ Validação de quantidade mínima de dados enviados

### Segurança e Middleware
- ✅ Middleware de erro global
- ✅ Retorno padronizado com `ErrorResponse`
- ✅ Registro de logs com `RequestId` único
- ✅ Token JWT com verificação de permissões por endpoint

---

## 🧪 Testes

- ✅ Testes unitários completos de:
  - Regras de domínio
  - Hash de senhas
  - Autenticação
  - Serviços e repositórios mockados
- ✅ Cobertura de cenários felizes e inválidos

---

## 🛠️ Setup do Projeto

### Pré-requisitos
- .NET 8 SDK instalado
- SQL Server

- Clonar o repositório
- Configurar a conexão com o banco de dados no `appsettings.json`
- Executar as migrations para criar o banco de dados:
  ```bash
  dotnet ef database update
  ```
- Executar a aplicação
  ```bash
  dotnet run --project FCG.API
  ```
- Acessar a documentação Swagger em `http://localhost:<porta>/swagger`
- 
---
 ## 🔐 Autenticação e Autorização

- Faça login com um usuário existente via /auth/login
- Use o token Bearer retornado no header Authorization das demais requisições protegidas.

---
 ## 📁 Estrutura de Pastas

 ```bash
/FCG.API				>> API principal com controllers e configuração de middleware
/FCG.Application		>> Camada de aplicação com serviços e DTOs
/FCG.Domain				>> Camada de domínio com entidades, repositórios e regras de negócio
/FCG.Infrastructure		>> Camada de infraestrutura com acesso a dados e implementação de repositórios
/FCG.Tests				>> Projeto de testes unitários
 ```

 ---
 ## ✍️ Autor
Frank Vieira
GitHub: @fkwesley
Projeto desenvolvido para fins educacionais no curso da FIAP.
 
