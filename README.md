# Order Processing Platform

Backend para processamento assíncrono de pedidos construído com **.NET 10**, **PostgreSQL**, **AWS SQS** e **AWS Lambda**.

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
  ├── SQS - payment.requested
  ├── SQS - inventory.requested
  └── SQS - notification.requested (planned)
```

O fluxo segue o padrão **Transactional Outbox**: a API persiste o pedido e um evento na tabela `OutboxMessages` na mesma transação. Um worker separado lê esses eventos e os publica no SQS. Lambdas consomem as filas para processar pagamento, estoque e notificações de forma independente.

## Tecnologias

- **ASP.NET Core 10** — API REST
- **MediatR** — CQRS (Commands/Handlers)
- **Entity Framework Core + Npgsql** — acesso a dados com PostgreSQL
- **PostgreSQL** — banco de dados principal
- **AWS SQS** — mensageria assíncrona
- **AWS Lambda (.NET 10)** — processamento de eventos

## Estrutura do projeto

```
src/
 ├── OrderProcessing.Api/            # Controllers, Program.cs
 ├── OrderProcessing.Application/    # Commands, Handlers (CQRS)
 ├── OrderProcessing.Domain/         # Entidades, Enums
 ├── OrderProcessing.Infrastructure/ # EF Core, DbContext, Migrations
 ├── OrderProcessing.Contracts/      # Payloads compartilhados entre projetos
 ├── Workers/
 │   └── OutboxPublisher.Worker/     # Background service — publica eventos no SQS
 └── Functions/
     └── ProcessOrder.Function/      # Lambda — consome order.created e roteia para payment/inventory
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [AWS CLI](https://aws.amazon.com/cli/) configurado com credenciais válidas
- [Amazon.Lambda.Tools](https://github.com/aws/aws-extensions-for-dotnet-cli) — `dotnet tool install -g Amazon.Lambda.Tools`

## Como executar

### 1. Subir o banco de dados

```bash
docker compose up -d
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

### 4. Rodar o OutboxPublisher Worker

```bash
dotnet run --project src/Workers/OutboxPublisher.Worker
```

### 5. Deploy da Lambda (ProcessOrder.Function)

```bash
cd src/Functions/ProcessOrder.Function
dotnet lambda package -c Release -o ./publish/function.zip
```

Faça upload do `function.zip` no Console da AWS e configure as variáveis de ambiente:

| Variável | Descrição |
|---|---|
| `INVENTORY_QUEUE_URL` | URL da fila SQS de inventory |
| `PAYMENT_QUEUE_URL` | URL da fila SQS de payment |

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
      "quantity": 2,
      "unitPrice": 49.90
    }
  ]
}
```

**Resposta:** `202 Accepted`
