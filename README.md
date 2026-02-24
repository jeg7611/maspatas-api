# MasPatas - Clean Architecture ASP.NET Core Web API (.NET 8)

Production-ready Web API with Clean Architecture and MongoDB.

## Solution structure

- `MasPatas.API`: Presentation layer (controllers, middleware, auth setup, Swagger).
- `MasPatas.Application`: Use cases/services, DTOs, validators, mapping profiles, abstractions.
- `MasPatas.Domain`: Core entities.
- `MasPatas.Infrastructure`: MongoDB persistence, repositories, security and resilience implementations.

## MongoDB configuration

Configuration is in `src/MasPatas.API/appsettings.json`:

```json
"MongoDb": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "MasPatasDb"
}
```

## Ventas: modelo y trazabilidad

### Colecciones

- `Sales`: venta, estado, pagos embebidos, versión optimista.
- `AuditLogs`: trazabilidad de acciones (`Sell`, `Pay`, `Cancel`) con `UserId` y `RequestId`.
- `IdempotencyRecords`: control de idempotencia por `RequestId` + operación.

### Estados

- `PendingPayment`
- `Paid`
- `Cancelled`
- `Refunded` (extensible)

### Endpoints

- `POST /api/sales/sell`
- `POST /api/sales/pay`
- `POST /api/sales/cancel`

#### Ejemplo `sell`

```json
{
  "customerId": "b8b76f6b-cf80-4f6d-9e86-59a98ba6e621",
  "items": [
    {
      "productId": "d2eb6648-8183-48d6-a7f0-08f69759b574",
      "quantity": 2,
      "price": 149.99
    }
  ],
  "requestId": "sell-req-10001",
  "userId": "6bd500d0-c6dc-4299-a113-2a6029a01f8c"
}
```

#### Ejemplo `pay`

```json
{
  "saleId": "ba6f340f-1a63-4f8d-8a01-06f5419b8b74",
  "paymentMethod": "Card",
  "amount": 299.98,
  "requestId": "pay-req-10001",
  "userId": "6bd500d0-c6dc-4299-a113-2a6029a01f8c"
}
```

#### Documento `Sales` (ejemplo)

```json
{
  "id": "ba6f340f-1a63-4f8d-8a01-06f5419b8b74",
  "customerId": "b8b76f6b-cf80-4f6d-9e86-59a98ba6e621",
  "items": [
    { "productId": "d2eb6648-8183-48d6-a7f0-08f69759b574", "quantity": 2, "unitPrice": 149.99 }
  ],
  "totalAmount": 299.98,
  "status": "Paid",
  "payments": [
    {
      "paymentId": "eb311185-f293-4877-8abe-f31f01dd4f93",
      "saleId": "ba6f340f-1a63-4f8d-8a01-06f5419b8b74",
      "paymentMethod": "Card",
      "amount": 299.98,
      "paidAt": "2026-01-31T18:11:00Z",
      "requestId": "pay-req-10001"
    }
  ],
  "version": 2,
  "createdAt": "2026-01-31T18:10:00Z",
  "updatedAt": "2026-01-31T18:11:00Z"
}
```

## Notes

- API uses role-based policies:
  - `AdminOnly`: product and user management.
  - `SellerOrAdmin`: sales and inventory movements.
- Ventas y pagos se ejecutan con transacciones MongoDB (`WithTransactionAsync`).
- Se aplica resiliencia con Polly (`Retry`, `CircuitBreaker`, `Bulkhead`, `Timeout` + fallback controlado).
