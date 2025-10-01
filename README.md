# OrderFlow API

OrderFlow é uma API desenvolvida em ASP.NET Core (.NET 8) para gerenciamento de pedidos e ocorrências, utilizando PostgreSQL como banco de dados e autenticação JWT para segurança.

## Funcionalidades

- Cadastro, consulta e gerenciamento de pedidos
- Cadastro, consulta e exclusão de ocorrências relacionadas aos pedidos
- Autenticação via JWT
- Documentação interativa via Swagger
- Logs detalhados com Serilog

## Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core (PostgreSQL)
- JWT Authentication
- Serilog (Logs)
- Swagger (Documentação)
- Injeção de Dependência

## Estrutura do Projeto

- `OrderFlow.Api`: Projeto principal da API
- `OrderFlow.Application`: Handlers e Dtos para regras de negócio
- `OrderFlow.Domain`: Interfaces e entidades de domínio
- `OrderFlow.Infra`: Implementação de repositórios e contexto de dados
- `OrderFlow.Tests`: Testes unitários

🚀 Como rodar com Docker

Pré-requisitos:

Docker + Docker Compose

Porta 7267 livre para a API + Swagger

Porta 5433 livre para o Postgres

Passo a passo
```
Clonar o repositório: git clone https://github.com/weslleytr/Pedidos-Ocorrencias.git
cd Pedidos-Ocorrencias
Subir o container: docker compose up -d --build
```
(Somente na 1ª vez) Rodar migrations Se você acabou de clonar, aplique as migrations antes de subir os containers.

Localmente (requer SDK .NET instalado):
```
dotnet tool restore
dotnet restore
dotnet build
cd .\OrderFlow.Api\
dotnet ef database update --project ../OrderFlow.Infra
```
API: http://localhost:7267

Swagger: http://localhost:7267/swagger

Postgres: localhost,5432

## Testes

Os testes unitários estão localizados em `OrderFlow.Tests`. Para executá-los:


## Autenticação

A API utiliza JWT. Para acessar endpoints protegidos, obtenha um token e inclua no header:


## Logs

Os logs são gerados no console e em arquivos diários na pasta `logs/`.

## Contribuição

Sinta-se à vontade para abrir issues ou pull requests.

## Licença

Este projeto está sob a licença MIT.

---

> Para dúvidas ou sugestões, entre em contato pelo [GitHub](https://github.com/weslleytr/Pedidos-Ocorrencias).
