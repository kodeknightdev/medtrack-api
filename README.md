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

Controller tests use the EF Core in-memory provider, so no database is needed.
