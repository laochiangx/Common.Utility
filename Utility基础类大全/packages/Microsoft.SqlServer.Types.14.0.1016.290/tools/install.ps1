param($installPath, $toolsPath, $package, $project)

$packagePath = (New-Object system.IO.DirectoryInfo $toolsPath).Parent.FullName
$cppBinaryPathx86 = Join-Path $packagePath "nativeBinaries\x86\msvcr120.dll"
$cppBinaryPathx64 = Join-Path $packagePath "nativeBinaries\x64\msvcr120.dll"
$sqlBinaryPathx86 = Join-Path $packagePath "nativeBinaries\x86\SqlServerSpatial140.dll"
$sqlBinaryPathx64 = Join-Path $packagePath "nativeBinaries\x64\SqlServerSpatial140.dll"

$sqlServerTypes = $project.ProjectItems.Item("SqlServerTypes")

$isAspNetProject = !$project.FullName.EndsWith('proj')

$folderx86 = $sqlServerTypes.ProjectItems | where Name -eq "x86"
if (!$folderx86)
{
    $folderx86 = $sqlServerTypes.ProjectItems.AddFolder("x86")
}

$folderx64 = $sqlServerTypes.ProjectItems | where Name -eq "x64"
if (!$folderx64)
{
    $folderx64 = $sqlServerTypes.ProjectItems.AddFolder("x64")
}

$cppLinkx86 = $folderx86.ProjectItems | where Name -eq "msvcr120.dll"
if (!$cppLinkx86)
{
    $cppLinkx86 = $folderx86.ProjectItems.AddFromFileCopy($cppBinaryPathx86)
    if (!$isAspNetProject)
    {
        $cppLinkx86.Properties.Item("CopyToOutputDirectory").Value = 2
    }
}

$sqlLinkx86 = $folderx86.ProjectItems | where Name -eq "SqlServerSpatial140.dll"
if (!$sqlLinkx86)
{
    $sqlLinkx86 = $folderx86.ProjectItems.AddFromFileCopy($sqlBinaryPathx86)
    if (!$isAspNetProject)
    {
        $sqlLinkx86.Properties.Item("CopyToOutputDirectory").Value = 2
    }
}

$cppLinkx64 = $folderx64.ProjectItems | where Name -eq "msvcr120.dll"
if (!$cppLinkx64)
{
    $cppLinkx64 = $folderx64.ProjectItems.AddFromFileCopy($cppBinaryPathx64)
    if (!$isAspNetProject)
    {
        $cppLinkx64.Properties.Item("CopyToOutputDirectory").Value = 2
    }
}

$sqlLinkx64 = $folderx64.ProjectItems | where Name -eq "SqlServerSpatial140.dll"
if (!$sqlLinkx64)
{
    $sqlLinkx64 = $folderx64.ProjectItems.AddFromFileCopy($sqlBinaryPathx64)
    if (!$isAspNetProject)
    {
        $sqlLinkx64.Properties.Item("CopyToOutputDirectory").Value = 2
    }
}

$readmefile = Join-Path $project.FullName "SqlServerTypes\readme.htm"
if (!$isAspNetProject)
{
    $readmefile = Join-Path (Split-Path $project.FullName) "SqlServerTypes\readme.htm"
}
$dte.ItemOperations.Navigate($readmefile)