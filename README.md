# MedTrack API

Backend for MedTrack, a small personal health journal: symptoms, medications
and appointments, plus a combined timeline.

Built with ASP.NET Core 3.1, EF Core 3.1 and SQL Server (LocalDB is fine).
The Angular client lives in a separate repo (`medtrack-web`).

## Getting started

Requirements: .NET Core SDK 3.1, SQL Server LocalDB (or change the connection string).

```
dotnet tool install --global dotnet-ef --version 3.1.8
dotnet ef database update --project src/MedTrack.Api
dotnet run --project src/MedTrack.Api
```

The API listens on `https://localhost:5001`. In Development, Swagger UI is at
`/swagger`.

`Jwt:Secret` is only set in `appsettings.Development.json`. For anything else set it
through an environment variable (`Jwt__Secret`) or user secrets.

## Tests

```
dotnet test
```

Controller tests use the EF Core in-memory provider, so no database is needed to run them.

## Endpoints

All endpoints except `/api/auth/*` and `/health` need a bearer token.

| Method | Route | Notes |
| --- | --- | --- |
| POST | `/api/auth/register` | returns a token |
| POST | `/api/auth/login` | returns a token |
| GET/POST | `/api/symptoms` | `?skip=&take=` paging on GET |
| GET/PUT/DELETE | `/api/symptoms/{id}` | |
| GET/POST | `/api/medications` | |
| GET/PUT/DELETE | `/api/medications/{id}` | |
| GET | `/api/medications/due` | reminders due today that haven't been taken |
| POST | `/api/medications/{id}/taken` | |
| GET/POST | `/api/appointments` | |
| GET/PUT/DELETE | `/api/appointments/{id}` | |
| GET | `/api/appointments/upcoming` | `?days=14` |
| GET | `/api/timeline` | `?from=&to=` |
| GET | `/health` | |
