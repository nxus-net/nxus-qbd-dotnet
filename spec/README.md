# OpenAPI Specification

This directory holds the OpenAPI v3 schema for the Nxus QuickBooks Desktop
API. The .NET SDK regenerates its `Nxus.Qbd.Models` namespace from this file
the same way the Python and TypeScript SDKs do.

## Source of Truth

`spec/openapi.json` is **not** hand-edited. It is exported from the backend
repository (`QbdWebService`), which is wired to NSwag at build time and emits
the spec from the controller signatures and DTOs.

## Refreshing the Spec

Run the backend's spec export task in the QbdWebService working copy:

```bash
# from QbdWebService root
dotnet run --project QbdWebService -- /spec
```

or, if the swagger UI is running locally:

```bash
curl https://localhost:7242/swagger/v1/swagger.json > spec/openapi.json
```

Then regenerate the models in this repo:

```bash
pwsh scripts/generate-models.ps1   # Windows / cross-platform PowerShell
bash scripts/generate-models.sh    # macOS / Linux
```

Commit the updated `spec/openapi.json` **and** the regenerated
`src/Nxus.Qbd/Models/Generated.cs` together. The two files form one logical
change — drift between them is a bug.

## Why Generate Instead of Copy?

The backend's `*.Application.Resources.**.Models.*.cs` DTOs are the gold
source, but copying them directly couples the SDK to the backend's
namespace/folder layout. Generating from the OpenAPI spec gives the SDK its
own namespace (`Nxus.Qbd.Models`) and the same regeneration ergonomics as
the Python and TS SDKs.
