# Typed Models

The .NET SDK is moving from the JsonElement-only surface returned by
`NxusHttpTransport` today to a typed `Nxus.Qbd.Models` namespace generated
from the same OpenAPI spec used by the Python and TypeScript SDKs.

## Why

The backend (`QbdWebService`) already maintains hand-tuned DTOs:

- `string?` for optional reference fields, value types where required
- `init`-only setters for immutability
- `[JsonPropertyName]` for camelCase mapping
- `<summary>` XML docs sourced from the QBD SDK

Python and TS both regenerate from `spec/openapi.json` on every release.
The .NET SDK lagged because it had no spec checked in and no generator wired
up, leaving callers to deserialize JsonElement by hand and losing null-safety
at the boundary.

## Generation Pipeline

```
QbdWebService/Application/Resources/**/Models/*.cs
        |
        | NSwag introspection at QbdWebService build time
        v
    spec/openapi.json  <-- checked into this repo
        |
        | nswag run nswag.json (scripts/generate-models.*)
        v
src/Nxus.Qbd/Models/Generated.cs   (Nxus.Qbd.Models.*)
```

Key NSwag settings (see `nswag.json`):

| Setting | Value | Rationale |
|---|---|---|
| `generateClientClasses` | `false` | We keep our own `NxusHttpTransport`; we only want the DTOs |
| `generateNullableReferenceTypes` | `true` | Mirrors backend `string?` nullability |
| `useRequiredKeyword` | `true` | Required fields become C# `required` properties |
| `propertySetterAccessModifier` | `""` (default public, but we use init) | Records with `init` setters |
| `jsonLibrary` | `SystemTextJson` | Matches transport's existing serializer |
| `dateType` / `dateTimeType` | `System.DateTimeOffset` | Avoids `DateTime` kind ambiguity |
| `requiredPropertiesMustBeDefined` | `true` | Forces callers to populate required fields |

## Null Handling on the Wire

The transport configures `System.Text.Json` with
`JsonIgnoreCondition.WhenWritingNull` so update requests omit unset properties
rather than sending explicit nulls. This is the same convention Python and TS
use and is what the backend's `JsonPatch`-style update endpoints expect.

When generation lands, `NxusHttpTransport` will gain typed overloads:

```csharp
// today
JsonElement vendor = await transport.GetAsync("/api/v1/vendors/123");

// after generation
Nxus.Qbd.Models.VendorDto vendor =
    await transport.GetAsync<Nxus.Qbd.Models.VendorDto>("/api/v1/vendors/123");
```

The JsonElement-returning methods stay in place during the transition for
backwards compatibility and for advanced callers using `transport.raw()`-style
escape hatches.

## Refreshing

See [`spec/README.md`](spec/README.md). Short form:

```bash
pwsh scripts/generate-models.ps1   # or scripts/generate-models.sh
```

Commit `spec/openapi.json` and `src/Nxus.Qbd/Models/Generated.cs` together.
