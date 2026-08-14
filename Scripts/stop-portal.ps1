#Requires -Version 5.1
<#
.SYNOPSIS
    Stops the Hardware Catalog React portal (Vite dev server) and releases its port.
#>
$ErrorActionPreference = 'SilentlyContinue'

$port = 5173
$stopped = $false

$processIds = Get-NetTCPConnection -LocalPort $port -State Listen |
    Select-Object -ExpandProperty OwningProcess -Unique

foreach ($processId in $processIds) {
    Write-Host "Stopping process $processId listening on port $port"
    Stop-Process -Id $processId -Force
    $stopped = $true
}

if (-not $stopped) {
    Write-Host "No process found listening on the portal port ($port)."
}
