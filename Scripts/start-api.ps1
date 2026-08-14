#Requires -Version 5.1
<#
.SYNOPSIS
    Starts the Hardware Catalog Web API.
#>
$ErrorActionPreference = 'Stop'

$apiProjectDir = Join-Path $PSScriptRoot '..\backend\HardwareCatalog.WebApi'

Push-Location $apiProjectDir
try {
    dotnet run --project .
}
finally {
    Pop-Location
}
