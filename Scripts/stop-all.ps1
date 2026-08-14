#Requires -Version 5.1
<#
.SYNOPSIS
    Stops both the Web API and the React portal, releasing their ports.
#>
$ErrorActionPreference = 'Stop'

& (Join-Path $PSScriptRoot 'stop-api.ps1')
& (Join-Path $PSScriptRoot 'stop-portal.ps1')
