#Requires -Version 5.1
<#
.SYNOPSIS
    Starts the Hardware Catalog React portal (Vite dev server).
#>
$ErrorActionPreference = 'Stop'

$frontendDir = Join-Path $PSScriptRoot '..\frontend'

Push-Location $frontendDir
try {
    npm run dev
}
finally {
    Pop-Location
}
