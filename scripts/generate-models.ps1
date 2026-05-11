#!/usr/bin/env pwsh
# Regenerate Nxus.Qbd.Models from spec/openapi.json using NSwag.
#
# Usage: pwsh scripts/generate-models.ps1
#
# Requires the dotnet SDK and the NSwag CLI tool:
#   dotnet tool install -g NSwag.ConsoleCore
#
# Reads:  spec/openapi.json
# Writes: src/Nxus.Qbd/Models/Generated.cs

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
Push-Location $repoRoot
try {
    if (-not (Test-Path 'spec/openapi.json')) {
        Write-Error 'spec/openapi.json is missing. See spec/README.md for how to export it from QbdWebService.'
    }

    if (-not (Get-Command nswag -ErrorAction SilentlyContinue)) {
        Write-Error "nswag CLI not found. Install with: dotnet tool install -g NSwag.ConsoleCore"
    }

    Write-Host 'Generating Nxus.Qbd.Models from spec/openapi.json...'
    nswag run nswag.json
    Write-Host 'Wrote src/Nxus.Qbd/Models/Generated.cs'
}
finally {
    Pop-Location
}
