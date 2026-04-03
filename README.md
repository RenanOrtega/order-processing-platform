# Order Processing Platform

Backend para processamento assíncrono de pedidos construído com **.NET 9**, **PostgreSQL**, **AWS SQS** e **Lambda**.

## Arquitetura

```
Client
  ↓
ASP.NET Core API
  ↓
PostgreSQL
  ├── Orders
  └── OutboxMessages
  ↓
OutboxPublisher Worker
  ↓
SQS - order.created
  ↓
ProcessOrder Lambda
  ├── payment.requested
  ├── inventory.requested
  └── notification.requested
```

O fluxo segue o padrão **Transactional Outbox**: a API persiste o pedido e um evento na tabela `OutboxMessages` na mesma transação. Um worker separado lê esses eventos e os publica no SQS. Lambdas consomem as filas para processar pagamento, estoque e notificações de forma independente.

## Tecnologias

- **ASP.NET Core 9** — API REST
- **MediatR** — CQRS (Commands/Handlers)
- **Entity Framework Core + Npgsql** — acesso a dados com PostgreSQL
- **PostgreSQL** — banco de dados principal
- **AWS SQS** — mensageria assíncrona *(planejado)*
- **AWS Lambda** — processamento de eventos *(planejado)*

## Estrutura do projeto

```
src/
 ├── OrderProcessing.Api/            # Controllers, Program.cs
 ├── OrderProcessing.Application/    # Commands, Handlers (CQRS)
 ├── OrderProcessing.Domain/         # Entidades, Enums
 └── OrderProcessing.Infrastructure/ # EF Core, DbContext, Migrations
```

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

## Como executar

### 1. Subir o banco de dados

```bash
docker-compose up -d
```

Isso sobe um container PostgreSQL com:
- **Host:** localhost:5432
- **Database:** orders
- **User:** postgres
- **Password:** password

### 2. Aplicar as migrations

```bash
dotnet ef database update --project src/OrderProcessing.Infrastructure --startup-project src/OrderProcessing.Api
```

### 3. Rodar a API

```bash
dotnet run --project src/OrderProcessing.Api
```

A API estará disponível em `https://localhost:{porta}`.

## Endpoints disponíveis

### Criar pedido

```http
POST /api/v1/orders
Content-Type: application/json

{
  "customerId": "customer-123",
  "items": [
    {
      "productId": "product-abc",
      "quantity": 2
    }
  ]
}
```

**Resposta:** `202 Accepted`
