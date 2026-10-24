# The Dating App

An ASP.NET Core Web API with an Angular client. This is an early version of the project: the API serves a list of users from SQLite and the client displays them. (A fuller version with auth, profiles and photos lives in [Milindpatel/DatingApp](https://github.com/Milindpatel/DatingApp).)

![CI](https://github.com/Milindpatel/Dating-App/actions/workflows/ci.yml/badge.svg)

## Tech stack

- **API:** .NET 10, ASP.NET Core controllers, EF Core 10 + SQLite, built-in OpenAPI document
- **Client:** Angular 22 (standalone, zoneless, signals), Bootstrap 5
- **Tests:** xUnit v3 integration tests, Vitest for the client

## Getting started

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download) and Node.js 22.22.3+ or 24+.

```bash
# API (https://localhost:5001)
cd API
dotnet dev-certs https --trust   # once, for the HTTPS dev certificate
dotnet run

# Client (http://localhost:4200)
cd client
npm install
npm start
```

On first run the database is created and seeded with a few demo users. In development the OpenAPI document is at `https://localhost:5001/openapi/v1.json`.

## API

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/users` | All users |
| GET | `/api/users/{id}` | One user (404 if missing) |

## Tests

```bash
dotnet test --solution DatingApp.slnx
cd client && npm test
```
