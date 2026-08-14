#Requires -Version 5.1
<#
.SYNOPSIS
    Starts both the Web API and the React portal, each in its own window.
#>
$ErrorActionPreference = 'Stop'

Start-Process powershell -ArgumentList '-NoExit', '-File', (Join-Path $PSScriptRoot 'start-api.ps1')
Start-Process powershell -ArgumentList '-NoExit', '-File', (Join-Path $PSScriptRoot 'start-portal.ps1')
