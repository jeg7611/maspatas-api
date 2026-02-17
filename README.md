# MasPatas - Clean Architecture ASP.NET Core Web API (.NET 8)

Production-ready Web API with Clean Architecture and MongoDB.

## Solution structure

- `MasPatas.API`: Presentation layer (controllers, middleware, auth setup, Swagger).
- `MasPatas.Application`: Use cases/services, DTOs, validators, mapping profiles, abstractions.
- `MasPatas.Domain`: Core entities.
- `MasPatas.Infrastructure`: MongoDB persistence, repositories, security implementations.

## MongoDB configuration

Configuration is in `src/MasPatas.API/appsettings.json`:

```json
"MongoDb": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "MasPatasDb"
}
```

Update values for your environment (production should use secure secrets and managed config providers).

## JWT configuration

```json
"Jwt": {
  "Key": "<strong-secret>",
  "Issuer": "MasPatas.API",
  "Audience": "MasPatas.Client",
  "ExpiresMinutes": 120
}
```

## Notes

- MongoDB uses one collection per entity (`Products`, `Customers`, `Inventory`, `InventoryMovements`, `Sales`, `Users`).
- API uses role-based policies:
  - `AdminOnly`: product and user management.
  - `SellerOrAdmin`: sales and inventory movements.
- Global exception middleware normalizes API error responses.
