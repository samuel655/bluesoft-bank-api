# 🏦 Bluesoft Bank API

Sistema bancario desarrollado con **.NET 9**, arquitectura **DDD + Hexagonal** y patrón **CQRS con MediatR**.

---

## 📐 Diagrama de Clases

![Diagrama de Clases UML - Bluesoft Bank](./diagrama_clases.png)

---

## 🏗️ Arquitectura

El proyecto sigue los principios de **Domain-Driven Design (DDD)** con **Arquitectura Hexagonal**, todo dentro de un único proyecto organizado por capas.

```
BluesoftBank/
├── Domain/                         # Núcleo del negocio — sin dependencias externas
│   ├── Entities/                   # Entidades y agregados del dominio
│   ├── Ports/                      # Interfaces (repositorios y servicios)
│   └── Exceptions/                 # Excepciones de dominio
│
├── Application/                    # Casos de uso — orquesta el dominio
│   └── CQRS/
│       ├── Commands/               # Operaciones que modifican estado
│       │   ├── Consignar/
│       │   ├── Retirar/
│       │   └── CrearCuenta/
│       └── Queries/                # Operaciones de solo lectura
│           ├── ConsultarSaldo/
│           ├── MovimientosRecientes/
│           ├── ExtractoMensual/
│           └── Reportes/
│
├── Infrastructure/                 # Implementaciones técnicas
│   └── Persistence/
│       ├── BankDbContext.cs        # EF Core DbContext
│       ├── Configurations/         # Fluent API por entidad (IEntityTypeConfiguration)
│       ├── Repositories/           # Implementación de los puertos del dominio
│       └── Migrations/             # Migraciones EF Core
│
└── API/                            # Capa de presentación
    ├── Controllers/                # Endpoints REST
    └── Middleware/                 # Manejo global de excepciones
```

---

## 🧩 Capas

### Domain
Contiene las reglas de negocio puras, sin dependencia de ningún framework. Las entidades principales son:

- **Cliente** *(abstract)* — base para `PersonaNatural` y `Empresa`
- **Cuenta** *(abstract)* — base para `CuentaAhorro` y `CuentaCorriente`. Contiene las reglas de negocio de saldo.
- **Transaccion** *(abstract)* — base para `TransaccionConsignacion`, `TransaccionRetiro` y `TransaccionTransferencia`
- **TipoTransaccion** *(enum)* — `Consignacion` | `Retiro`

Reglas de negocio aplicadas directamente en las entidades:
- ✅ El saldo **nunca puede ser negativo**
- ✅ Toda operación concurrente sobre una cuenta es **consistente** (optimistic locking con `RowVersion`)

### Application — CQRS con MediatR
Separa claramente las operaciones de escritura y lectura:

| Tipo | Clase | Descripción |
|------|-------|-------------|
| Command | `ConsignarCommand` | Registra una consignación en la cuenta |
| Command | `RetirarCommand` | Registra un retiro validando saldo |
| Command | `CrearCuentaCommand` | Crea una cuenta de ahorro o corriente |
| Query | `ConsultarSaldoQuery` | Retorna el saldo actual de una cuenta |
| Query | `MovimientosRecientesQuery` | Últimos N movimientos de una cuenta |
| Query | `ExtractoMensualQuery` | Extracto de un mes específico |
| Query | `TransaccionesPorClienteQuery` | Reporte: clientes ordenados por # transacciones |
| Query | `RetirosFueraCiudadQuery` | Reporte: retiros fuera de ciudad con total > $1.000.000 |

### Infrastructure
- **EF Core 9** con **PostgreSQL** para persistencia principal
- **Fluent API** con clases `IEntityTypeConfiguration` separadas por entidad
- **Optimistic Locking** con `RowVersion` para garantizar consistencia en operaciones concurrentes
- **Dapper** para los queries analíticos de reportes en tiempo real
- Índices optimizados en `transacciones` para los reportes: `(cuenta_id, fecha)`, `fecha`, `tipo`, `ciudad_transaccion`

### API
- **Controllers REST** que solo dispatchen Commands/Queries via `IMediator`
- **Middleware global de excepciones** que traduce excepciones de dominio a HTTP status codes

```
SaldoInsuficienteException  → 422 Unprocessable Entity
CuentaNoEncontradaException → 404 Not Found
ConcurrenciaException       → 409 Conflict
MontoInvalidoException      → 400 Bad Request
```

---

## 🛠️ Tecnologías

| Tecnología | Uso |
|------------|-----|
| .NET 9 | Framework principal |
| ASP.NET Core | API REST |
| MediatR 12 | CQRS — Commands y Queries |
| Entity Framework Core 9 | ORM principal |
| Npgsql | Driver PostgreSQL para EF Core |
| Dapper | Queries SQL optimizadas para reportes |
| PostgreSQL | Base de datos |
| Swagger / Swashbuckle | Documentación de la API |

---

## 🚀 Cómo correr el proyecto

### Prerequisitos
- .NET 9 SDK
- PostgreSQL corriendo localmente

### Configuración

```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=bluesoft_bank_dev;Username=postgres;Password=postgres"
  }
}
```

### Comandos

```bash
# Restaurar dependencias
dotnet restore

# Aplicar migraciones
dotnet ef database update

# Correr el proyecto
dotnet run
```

La API queda disponible en `https://localhost:5001/swagger`

---

## 📡 Endpoints principales

### Cuentas
```
POST   /api/cuentas/{id}/consignar        → Consignar dinero
POST   /api/cuentas/{id}/retirar          → Retirar dinero
GET    /api/cuentas/{id}/saldo            → Consultar saldo
GET    /api/cuentas/{id}/movimientos      → Movimientos recientes
GET    /api/cuentas/{id}/extracto?mes=&anio=  → Extracto mensual
```

### Reportes
```
GET    /api/reportes/transacciones-por-cliente?mes=&anio=  → Clientes por # transacciones
GET    /api/reportes/retiros-fuera-ciudad                  → Retiros fuera de ciudad > $1.000.000
```

---

## 🔒 Consistencia y Concurrencia

Para garantizar que dos operaciones simultáneas sobre la misma cuenta no generen un saldo incorrecto, se usa **Optimistic Locking** con `RowVersion` de EF Core:

```
Operación A lee cuenta (version=5)
Operación B lee cuenta (version=5)
Operación A guarda     → version pasa a 6 ✅
Operación B intenta guardar con version=5 → EF Core lanza DbUpdateConcurrencyException
→ Middleware responde 409 Conflict → el cliente reintenta
```

Esto evita el uso de locks pesimistas (SELECT FOR UPDATE) y mantiene alta concurrencia.