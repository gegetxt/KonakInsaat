# KonakInsaat uygulamasını ve port 5006'yı kullanan tüm işlemleri durdurur

Write-Host "Port 5006'yı kullanan işlemler durduruluyor..." -ForegroundColor Yellow

# Port 5006'yı kullanan işlemleri bul ve durdur
$portConnections = Get-NetTCPConnection -LocalPort 5006 -ErrorAction SilentlyContinue
if ($portConnections) {
    $portConnections | ForEach-Object {
        $proc = Get-Process -Id $_.OwningProcess -ErrorAction SilentlyContinue
        if ($proc) {
            Write-Host "  - PID $($_.OwningProcess): $($proc.ProcessName)" -ForegroundColor Cyan
            Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue
        }
    }
}

# KonakInsaat ve dotnet işlemlerini durdur
Get-Process | Where-Object {
    $_.ProcessName -eq "KonakInsaat" -or 
    ($_.ProcessName -eq "dotnet" -and $_.Path -like "*KonakInsaat*")
} | ForEach-Object {
    Write-Host "  - PID $($_.Id): $($_.ProcessName)" -ForegroundColor Cyan
    Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
}

Start-Sleep -Seconds 2
Write-Host "`n✅ Tüm işlemler durduruldu. Artık 'dotnet run' komutunu çalıştırabilirsiniz." -ForegroundColor Green













