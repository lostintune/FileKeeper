# FileKeeper

A backend for a simplified Google Drive-style file storage (no folders), built with ASP.NET Core following Clean Architecture principles.

---

## What the application does

- Users can register (first name, last name, username, email, phone number, password) and log in (JWT-based authentication).
- A registered user can view, edit and soft-delete their own profile.
- A user can upload files, see the list of their own files, rename a file, download it and soft-delete it.
- A file owner can share a file with other users and revoke access.
- A user can see the list of files shared with them, open and rename them.

### Access rules

| Action | Owner | User the file is shared with |
|---|---|---|
| View, download, rename | yes | yes |
| Share, unshare, delete | yes | no |

---

## Architecture

Clean Architecture, four projects:

```
FileKeeper.Domain          — entities, domain exceptions, business rules
FileKeeper.Application     — CQRS (MediatR): commands, queries, handlers, validation, interfaces
FileKeeper.Infrastructure  — EF Core, PostgreSQL, Identity, JWT, local file storage
FileKeeper.API             — controllers, middleware, DI composition
```

**Dependency rule:** Domain depends on nothing. Application depends only on Domain and declares interfaces (`IFileRepository`, `IFileAccessRepository`, `IFileStorage`, `IIdentityService`, `IJwtTokenGenerator`) that are implemented in Infrastructure. API is the only layer aware of HTTP and wires everything together through dependency injection.

**Rich domain model.** `UserEntity`, `FileEntity` and `FileAccessEntity` have private setters and static factory methods (`Create`). Business rules live inside the entities (for example, a file cannot be shared with its creator or when it is deleted).

**CQRS via MediatR.** Every action is a separate command or query with its own handler:

```
Application/
├── Users/
│   ├── Commands/{RegisterUser, LoginUser, UpdateProfile, DeleteUser}
│   └── Queries/GetUserById
└── Files/
    ├── Commands/{CreateFile, UpdateFile, DeleteFile, ShareFile, UnshareFile}
    └── Queries/{GetFileById, GetMyFiles, GetSharedFiles, DownloadFile}
```

Controllers contain no business logic: they build a command or query, send it through `IMediator` and return the result.

**Two-level validation:**

- FluentValidation runs in a MediatR pipeline behavior (`ValidationBehaviour`) before the handler is called.
- Domain entities are the last line of defense and throw domain exceptions on invalid state.

**Identity.** Users are stored in ASP.NET Core Identity tables (`AppIdentityUser`). The domain `UserEntity` is mapped to and from it in `IdentityService`, so Domain and Application stay independent of Identity.

**Error handling.** A single `ExceptionHandlingMiddleware` turns exceptions into HTTP responses:

| Exception | Status |
|---|---|
| `FluentValidation.ValidationException` | 400, `{"errors": {"Field": ["message"]}}` |
| `DomainException` and subtypes, `IdentityOperationException` | 400 |
| `InvalidCredentialsException` | 401 |
| `ForbiddenAccessException` | 403 |
| `NotFoundException` and subtypes | 404 |
| `ConflictException` and subtypes | 409 |
| anything else | 500, generic message (details only in logs) |

All other errors use the format `{"error": "message"}`.

**Soft delete.** Users and files are never removed from the database: they are marked with `IsDeleted`, and global EF Core query filters hide them from all queries.

---

## Tech stack

- **.NET 10**, ASP.NET Core Web API
- **PostgreSQL** with EF Core (Npgsql)
- **ASP.NET Core Identity**: user storage and password hashing
- **JWT Bearer**: authentication
- **MediatR**: CQRS
- **FluentValidation**: command validation
- Local disk storage for uploaded files

---

## Getting started

### 1. Prerequisites

- .NET 10 SDK
- PostgreSQL (local or containerized)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### 2. Configuration

`appsettings.json` is a template and contains no secrets. Local settings go into `FileKeeper.API/appsettings.Development.json`, which is excluded from git:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=FileKeeperDb;Username=<user>;Password=<password>"
  },
  "JwtOptions": {
    "SecretKey": "<random string, at least 32 characters>"
  }
}
```

> The JWT secret must be at least 32 characters long. You can generate one with `openssl rand -base64 32`.

### 3. Apply database migrations

From the solution root:

```bash
dotnet ef database update --project FileKeeper.Infrastructure --startup-project FileKeeper.API
```

This creates the `FileKeeperDb` database if it does not exist.

### 4. Run

```bash
dotnet run --project FileKeeper.API
```

The API listens on `http://localhost:5171` (check the `Now listening on:` line in the console). The OpenAPI document is available at `/openapi/v1.json` in Development. Ready-made requests are in `FileKeeper.API/FileKeeper.API.http`.

### 5. Smoke test

1. `POST /api/users/register`: register a user.
2. `POST /api/users/login`: get a JWT.
3. Send `Authorization: Bearer <token>` with every following request.
4. `POST /api/files`: upload a file (`multipart/form-data`).
5. `GET /api/files/my`: check that it appears in the list.

---

## API endpoints

All endpoints except `register` and `login` require a JWT. The current user is always taken from the token, never from the request.

### Users

| Method | Path | Description |
|---|---|---|
| POST | `/api/users/register` | Register (public) |
| POST | `/api/users/login` | Log in, returns `{ token, expiresAt }` (public) |
| GET | `/api/users/me` | Get own profile |
| PUT | `/api/users/me` | Update own profile |
| DELETE | `/api/users/me` | Soft-delete own account |

### Files

| Method | Path | Description |
|---|---|---|
| POST | `/api/files` | Upload a file (form fields `Name`, `File`, max 10 MB) |
| GET | `/api/files/my` | Files I created |
| GET | `/api/files/shared` | Files shared with me |
| GET | `/api/files/{id}` | File details (owner or shared user) |
| GET | `/api/files/{id}/download` | Download a file (owner or shared user) |
| PUT | `/api/files/{id}` | Rename a file (owner or shared user) |
| DELETE | `/api/files/{id}` | Soft-delete a file (owner only) |
| POST | `/api/files/{id}/share` | Share with a user, body `{ "targetUserId": "..." }` (owner only) |
| DELETE | `/api/files/{id}/share/{targetUserId}` | Revoke access (owner only) |

---

## Notable design decisions

- **The user id is never accepted from the client.** It is read from the JWT through `CurrentUserService`, so one user cannot act as another by changing an id in a request.
- **Uploaded files are stored under a generated GUID name**, not the original name, so files with equal names never overwrite each other. The user-visible name lives in the database.
- **Uniqueness is enforced twice:** in handlers for friendly error messages, and by unique database indexes (email, username, `(UserId, FileId)`) as a safety net.
- **Entity configuration** is split into `IEntityTypeConfiguration` classes instead of one large `OnModelCreating`.

---

## Known limitations / possible next steps

- No rate limiting or account lockout on `/login`.
- JWTs cannot be revoked: a token of a deleted user keeps working until it expires (1 hour).
- Deleting a user does not affect their files or access records.
- Concurrent requests can hit a unique index and return 500 instead of 409 (for example, two parallel registrations with the same email).
- A user cannot register with the email or username of a deleted account (the query filter hides it from the check, but the unique index still blocks it).
- Sharing grants write access (rename); read-only sharing is not implemented.
- File extensions are not restricted; the 10 MB limit is checked after the request is read.
- Files are stored on local disk (`uploads/`), not in cloud storage, and the absolute path is stored in the database.
- Error responses use three different formats (validation, application errors, model binding).
- No automated tests yet.
- MediatR 14 requires a license key for commercial use.