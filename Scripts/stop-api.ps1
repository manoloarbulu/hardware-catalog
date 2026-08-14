#Requires -Version 5.1
<#
.SYNOPSIS
    Stops the Hardware Catalog Web API and releases its ports.
#>
$ErrorActionPreference = 'SilentlyContinue'

$ports = 5199, 7232
$stopped = $false

foreach ($port in $ports) {
    $processIds = Get-NetTCPConnection -LocalPort $port -State Listen |
        Select-Object -ExpandProperty OwningProcess -Unique

    foreach ($processId in $processIds) {
        Write-Host "Stopping process $processId listening on port $port"
        Stop-Process -Id $processId -Force
        $stopped = $true
    }
}

if (-not $stopped) {
    Write-Host 'No process found listening on the API ports (5199, 7232).'
}
