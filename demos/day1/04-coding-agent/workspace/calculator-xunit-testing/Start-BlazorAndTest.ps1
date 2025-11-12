<#
.SYNOPSIS
    Automates Blazor Server startup and Playwright test execution for the Calculator application.

.DESCRIPTION
    This script automates the complete test execution workflow:
    1. Builds the Calculator.Web project
    2. Starts the Blazor Server application in the background
    3. Waits for the health endpoint to respond
    4. Executes Playwright tests
    5. Captures test results
    6. Stops the application process
    7. Exits with the appropriate test result code

.PARAMETER Configuration
    Build configuration for the .NET project. Default is "Debug".

.PARAMETER Headed
    Run tests in headed browser mode (visible browser windows) for debugging.

.PARAMETER Browser
    Specify which browser to test. Options: "all", "chromium", "firefox", "webkit".
    Default is "all".

.PARAMETER Project
    Specific test project to run (chromium, firefox, webkit). Overrides Browser parameter.

.EXAMPLE
    .\Start-BlazorAndTest.ps1
    Run all tests in headless mode with Debug configuration.

.EXAMPLE
    .\Start-BlazorAndTest.ps1 -Headed
    Run all tests in headed mode (visible browsers).

.EXAMPLE
    .\Start-BlazorAndTest.ps1 -Browser chromium
    Run tests only in Chromium browser.

.EXAMPLE
    .\Start-BlazorAndTest.ps1 -Configuration Release -Project firefox
    Run tests in Firefox with Release configuration.

.NOTES
    File Name      : Start-BlazorAndTest.ps1
    Author         : GitHub Copilot
    Prerequisite   : .NET 9.0 SDK, Node.js, Playwright browsers installed
    Version        : 1.0
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    
    [Parameter(Mandatory=$false)]
    [switch]$Headed,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("all", "chromium", "firefox", "webkit")]
    [string]$Browser = "all",
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("chromium", "firefox", "webkit")]
    [string]$Project
)

# Script configuration
$ErrorActionPreference = "Stop"
$webProjectPath = Join-Path $PSScriptRoot "calculator.web"
$healthEndpoint = "https://localhost:5001/health"
$maxWaitSeconds = 30
$healthCheckInterval = 2

# Color output functions
function Write-Step {
    param([string]$Message)
    Write-Host "`n==> $Message" -ForegroundColor Cyan
} # end Write-Step

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
} # end Write-Success

function Write-Failure {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
} # end Write-Failure

function Write-Info {
    param([string]$Message)
    Write-Host "  $Message" -ForegroundColor Gray
} # end Write-Info

# Cleanup function
function Stop-BlazorServer {
    param([System.Diagnostics.Process]$Process)
    
    if ($Process -and -not $Process.HasExited) {
        Write-Step "Stopping Blazor Server..."
        try {
            Stop-Process -Id $Process.Id -Force -ErrorAction SilentlyContinue
            Start-Sleep -Seconds 2
            Write-Success "Blazor Server stopped"
        } catch {
            Write-Warning "Failed to stop Blazor Server: $_"
        } # end try/catch
    } # end if
} # end Stop-BlazorServer

# Main execution
try {
    Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║  Calculator Blazor Server - Playwright Test Automation    ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
    
    # Step 1: Build the project
    Write-Step "Building Calculator.Web project ($Configuration)..."
    Write-Info "Project path: $webProjectPath"
    
    if (-not (Test-Path $webProjectPath)) {
        Write-Failure "Calculator.Web project not found at: $webProjectPath"
        exit 1
    } # end if
    
    $buildOutput = dotnet build "$webProjectPath" --configuration $Configuration 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Failure "Build failed"
        Write-Host $buildOutput
        exit 1
    } # end if
    Write-Success "Build completed successfully"
    
    # Step 2: Start Blazor Server in background
    Write-Step "Starting Blazor Server..."
    Write-Info "Endpoint: https://localhost:5001"
    
    $processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $processStartInfo.FileName = "dotnet"
    $processStartInfo.Arguments = "run --project `"$webProjectPath`" --configuration $Configuration --no-build"
    $processStartInfo.UseShellExecute = $false
    $processStartInfo.RedirectStandardOutput = $true
    $processStartInfo.RedirectStandardError = $true
    $processStartInfo.CreateNoWindow = $true
    $processStartInfo.WorkingDirectory = $webProjectPath
    
    $process = [System.Diagnostics.Process]::Start($processStartInfo)
    Write-Info "Process ID: $($process.Id)"
    
    # Start reading output in background to prevent buffer blocking
    $outputBuilder = New-Object System.Text.StringBuilder
    $errorBuilder = New-Object System.Text.StringBuilder
    
    $outputEvent = Register-ObjectEvent -InputObject $process -EventName OutputDataReceived -Action {
        if ($EventArgs.Data) {
            [void]$Event.MessageData.AppendLine($EventArgs.Data)
        } # end if
    } -MessageData $outputBuilder
    
    $errorEvent = Register-ObjectEvent -InputObject $process -EventName ErrorDataReceived -Action {
        if ($EventArgs.Data) {
            [void]$Event.MessageData.AppendLine($EventArgs.Data)
        } # end if
    } -MessageData $errorBuilder
    
    $process.BeginOutputReadLine()
    $process.BeginErrorReadLine()
    
    # Step 3: Wait for health endpoint
    Write-Step "Waiting for health endpoint to respond..."
    Write-Info "Health check: $healthEndpoint"
    Write-Info "Max wait time: $maxWaitSeconds seconds"
    
    $elapsed = 0
    $isHealthy = $false
    
    while ($elapsed -lt $maxWaitSeconds) {
        try {
            # Ignore SSL certificate errors for localhost
            [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
            $response = Invoke-WebRequest -Uri $healthEndpoint -UseBasicParsing -TimeoutSec 2 -ErrorAction SilentlyContinue
            
            if ($response.StatusCode -eq 200) {
                $isHealthy = $true
                Write-Success "Health endpoint responded successfully"
                break
            } # end if
        } catch {
            # Connection failed, continue waiting
        } # end try/catch
        
        Write-Host "." -NoNewline -ForegroundColor Yellow
        Start-Sleep -Seconds $healthCheckInterval
        $elapsed += $healthCheckInterval
    } # end while
    
    Write-Host "" # New line after dots
    
    if (-not $isHealthy) {
        Write-Failure "Health endpoint did not respond within $maxWaitSeconds seconds"
        Write-Host "`nServer Output:" -ForegroundColor Yellow
        Write-Host $outputBuilder.ToString()
        if ($errorBuilder.Length -gt 0) {
            Write-Host "`nServer Errors:" -ForegroundColor Red
            Write-Host $errorBuilder.ToString()
        } # end if
        Unregister-Event -SourceIdentifier $outputEvent.Name -ErrorAction SilentlyContinue
        Unregister-Event -SourceIdentifier $errorEvent.Name -ErrorAction SilentlyContinue
        Stop-BlazorServer -Process $process
        exit 1
    } # end if
    
    Write-Info "Server is ready!"
    
    # Step 4: Execute Playwright tests
    Write-Step "Executing Playwright tests..."
    
    # Build test command
    $testArgs = @("test")
    
    if ($Headed) {
        $testArgs += "--headed"
        Write-Info "Mode: Headed (visible browsers)"
    } else {
        Write-Info "Mode: Headless"
    } # end if/else
    
    if ($Project) {
        $testArgs += "--project=$Project"
        Write-Info "Browser: $Project"
    } elseif ($Browser -ne "all") {
        $testArgs += "--project=$Browser"
        Write-Info "Browser: $Browser"
    } else {
        Write-Info "Browsers: Chromium, Firefox, WebKit"
    } # end if/else
    
    Write-Info "Command: npm $($testArgs -join ' ')"
    Write-Host ""
    
    # Run tests
    $testExitCode = 0
    try {
        npm @testArgs
        $testExitCode = $LASTEXITCODE
    } catch {
        Write-Failure "Test execution failed: $_"
        $testExitCode = 1
    } # end try/catch
    
    # Step 5: Capture results
    Write-Host ""
    Write-Step "Test Execution Complete"
    
    if ($testExitCode -eq 0) {
        Write-Success "All tests passed!"
        Write-Info "View detailed report: npm run test:report"
    } else {
        Write-Failure "Some tests failed (Exit code: $testExitCode)"
        Write-Info "View detailed report: npm run test:report"
    } # end if/else
    
    # Step 6: Stop server
    Write-Host ""
    Unregister-Event -SourceIdentifier $outputEvent.Name -ErrorAction SilentlyContinue
    Unregister-Event -SourceIdentifier $errorEvent.Name -ErrorAction SilentlyContinue
    Stop-BlazorServer -Process $process
    
    # Step 7: Exit with test result code
    Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║  Test automation completed                                 ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan
    
    exit $testExitCode
    
} catch {
    Write-Failure "An unexpected error occurred: $_"
    Write-Host $_.ScriptStackTrace
    
    # Ensure server is stopped
    if ($process) {
        Stop-BlazorServer -Process $process
    } # end if
    
    exit 1
} # end try/catch
