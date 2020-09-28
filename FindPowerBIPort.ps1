Get-ChildItem -Path "$($env:LocalAppData)/Microsoft/Power BI Desktop/AnalysisServicesWorkspaces" -Directory |
    Get-ChildItem -Filter msmdsrv.port.txt -Recurse |
    Sort-Object LastWriteTime -Descending |
    ForEach-Object {
        $timeStamp = $_.LastWriteTime
        $content = $_ | Get-Content
        Write-Output "$timeStamp - Port $content"
    }
