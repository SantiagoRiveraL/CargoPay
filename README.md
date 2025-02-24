# CargoPay 💳

## Overview
CargoPay is a modern payment provider's Authorization System, featuring robust Card Management and Payment Fees modules. Built with C# and RESTful principles, it offers secure and efficient payment processing capabilities.

## Features
- Card Management System
- Dynamic Payment Fee Calculator
- RESTful API Architecture
- Secure Authentication
- PostgreSQL Database Integration

## Tech Stack
- **Backend:** C# .NET
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL (Railway)
- **API Framework:** ASP.NET Core Web API
- **Architecture Patterns:**
  - Dependency Injection
  - Repository Pattern
  - Singleton Pattern (for Fee Calculation)

## Project Structure
```
CargoPay/
├── API/           # Controllers and endpoints
├── Core/          # Business logic and entities
└── Infrastructure/# Data persistence and repositories
```

## API Documentation

### Card Management Module

#### Create Card
```http
POST /api/cards
```
**Request Body:**
```json
{
    "cardNumber": "123456789012345",
    "initialBalance": 1000.00
}
```
**Response:**
```json
{
    "id": "guid",
    "cardNumber": "123456789012345",
    "balance": 1000.00,
    "createdAt": "2024-02-23T10:00:00Z",
    "userId": "guid"
}
```

#### Process Payment
```http
POST /api/cards/payment
```
**Request Body:**
```json
{
    "cardNumber": "123456789012345",
    "amount": 100.00
}
```
**Response:**
```json
{
    "cardNumber": "123456789012345",
    "amount": 100.00,
    "remainingBalance": 900.00,
    "transactionDate": "2024-02-23T10:05:00Z"
}
```

#### Get Card Balance
```http
GET /api/cards/{cardNumber}/balance
```
**Response:**
```json
{
    "cardNumber": "123456789012345",
    "balance": 900.00
}
```

### Payment Fees Module

The system implements a dynamic fee calculation system that:
- Generates random fees hourly
- Uses Singleton Pattern for Universal Fees Exchange (UFE)
- Calculates new fees using the formula: `New Fee = Last Fee * Random(0,2)`

## Advanced Features

### Performance Optimizations
- Multi-threading support for enhanced API performance
- Thread-safe resource handling
- Optimized database queries and ORM usage

### Security
- Enhanced authentication beyond basic auth
- Secure payment processing
- Data encryption protocols

## Getting Started

### Prerequisites
- .NET 6.0 or higher
- PostgreSQL
- Git

### Installation

1. Clone the repository
```bash
git clone https://github.com/SantiagoRiveraL/cargopay.git
```

2. Navigate to project directory
```bash
cd cargopay
```

3. Update database connection string in `appsettings.json`
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "XXXXXX"
    }
}
```

4. Run database migrations
```bash
dotnet ef database update
```

5. Start the API
```bash
dotnet run --project CargoPay.API
```

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
Santiago Rivera López - santiago.rivera.lpz@gmail.com 

Project Link: [https://github.com/SantiagoRiveraL/cargopay](https://github.com/SantiagoRiveraL/cargopay)
