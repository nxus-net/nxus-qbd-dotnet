#!/usr/bin/env bash
# Regenerate Nxus.Qbd.Models from spec/openapi.json using NSwag.
#
# Usage: bash scripts/generate-models.sh
#
# Requires the dotnet SDK and the NSwag CLI tool:
#   dotnet tool install -g NSwag.ConsoleCore
#
# Reads:  spec/openapi.json
# Writes: src/Nxus.Qbd/Models/Generated.cs

set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

if [ ! -f spec/openapi.json ]; then
  echo 'spec/openapi.json is missing. See spec/README.md for how to export it from QbdWebService.' >&2
  exit 1
fi

if ! command -v nswag >/dev/null 2>&1; then
  echo 'nswag CLI not found. Install with: dotnet tool install -g NSwag.ConsoleCore' >&2
  exit 1
fi

echo 'Generating Nxus.Qbd.Models from spec/openapi.json...'
nswag run nswag.json
echo 'Wrote src/Nxus.Qbd/Models/Generated.cs'
