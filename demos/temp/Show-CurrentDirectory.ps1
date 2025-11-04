<#
.SYNOPSIS
    Script to display the current working directory and list day1 folder contents.

.DESCRIPTION
    This script displays the current working directory and then lists all files 
    and directories within the day1 folder of the ghcp-hackathon-custom repository.
    It filters out hidden files, build artifacts, and common exclusions.

.EXAMPLE
    .\Show-CurrentDirectory.ps1
    
    Displays current directory and lists day1 folder contents.

.NOTES
    File Name      : Show-CurrentDirectory.ps1
    Author         : GitHub Copilot
    Prerequisite   : PowerShell 5.1 or higher
    Version        : 1.0
#>

function Show-CurrentDirectory
{
    <#
    .SYNOPSIS
        Display the current working directory.
    #>
    $currentDir = Get-Location
    Write-Host "Current directory: $currentDir" -ForegroundColor Cyan
    return $currentDir
} #end Show-CurrentDirectory

function Get-RepositoryContents
{
    <#
    .SYNOPSIS
        List all files and directories in the specified path.
    
    .PARAMETER Path
        Path to the directory to list. If not specified, uses current directory.
    #>
    param(
        [Parameter(Mandatory=$false)]
        [string]$Path = (Get-Location).Path
    )
    
    Write-Host "`n$('=' * 80)" -ForegroundColor Yellow
    Write-Host "Repository Contents: $Path" -ForegroundColor Yellow
    Write-Host "$('=' * 80)`n" -ForegroundColor Yellow
    
    # Get all items recursively
    $allItems = Get-ChildItem -Path $Path -Recurse -Force -ErrorAction SilentlyContinue
    
    # Filter out excluded items
    $excludedPatterns = @('.*', '__pycache__', 'node_modules', 'bin', 'obj')
    
    $filteredItems = $allItems | Where-Object {
        $item = $_
        $shouldExclude = $false
        
        foreach ($pattern in $excludedPatterns)
        {
            if ($item.FullName -like "*\$pattern\*" -or $item.FullName -like "*\$pattern")
            {
                $shouldExclude = $true
                break
            } #end if
            
            if ($item.Name -like $pattern)
            {
                $shouldExclude = $true
                break
            } #end if
        } #end foreach
        
        -not $shouldExclude
    } #end Where-Object
    
    # Separate directories and files
    $directories = $filteredItems | Where-Object { $_.PSIsContainer } | Sort-Object FullName
    $files = $filteredItems | Where-Object { -not $_.PSIsContainer } | Sort-Object FullName
    
    # Get relative paths
    $dirList = @()
    foreach ($dir in $directories)
    {
        $relativePath = $dir.FullName.Replace("$Path\", "")
        $dirList += $relativePath
    } #end foreach
    
    $fileList = @()
    foreach ($file in $files)
    {
        $relativePath = $file.FullName.Replace("$Path\", "")
        $fileList += $relativePath
    } #end foreach
    
    # Display directories
    Write-Host "Directories ($($dirList.Count)):" -ForegroundColor Green
    Write-Host "$('-' * 80)" -ForegroundColor Gray
    
    $displayLimit = 50
    $dirCount = 0
    foreach ($dir in $dirList)
    {
        if ($dirCount -lt $displayLimit)
        {
            Write-Host "  📁 $dir" -ForegroundColor White
            $dirCount++
        } #end if
        else
        {
            break
        } #end else
    } #end foreach
    
    if ($dirList.Count -gt $displayLimit)
    {
        Write-Host "  ... and $($dirList.Count - $displayLimit) more directories" -ForegroundColor Gray
    } #end if
    
    # Display files
    Write-Host "`nFiles ($($fileList.Count)):" -ForegroundColor Green
    Write-Host "$('-' * 80)" -ForegroundColor Gray
    
    $fileCount = 0
    foreach ($file in $fileList)
    {
        if ($fileCount -lt $displayLimit)
        {
            Write-Host "  📄 $file" -ForegroundColor White
            $fileCount++
        } #end if
        else
        {
            break
        } #end else
    } #end foreach
    
    if ($fileList.Count -gt $displayLimit)
    {
        Write-Host "  ... and $($fileList.Count - $displayLimit) more files" -ForegroundColor Gray
    } #end if
    
    Write-Host "`nTotal: $($dirList.Count) directories, $($fileList.Count) files" -ForegroundColor Cyan
} #end Get-RepositoryContents

# Main script execution
$current = Show-CurrentDirectory

# Find repository root
$repoRoot = $current
while ($repoRoot.Path -notmatch 'ghcp-hackathon-custom$' -and $repoRoot.Parent -ne $null)
{
    $repoRoot = Split-Path -Parent $repoRoot.Path
} #end while

if ($repoRoot -match 'ghcp-hackathon-custom')
{
    # List only the day1 folder
    $day1Path = Join-Path -Path $repoRoot -ChildPath 'demos\day1'
    
    if (Test-Path -Path $day1Path)
    {
        Get-RepositoryContents -Path $day1Path
    } #end if
    else
    {
        Write-Host "`nday1 folder not found at: $day1Path" -ForegroundColor Red
    } #end else
} #end if
else
{
    Write-Host "`nCould not find repository root. Listing current directory instead:" -ForegroundColor Yellow
    Get-RepositoryContents -Path $current.Path
} #end else
