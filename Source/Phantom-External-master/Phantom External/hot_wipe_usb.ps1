```powershell
# Script de Limpeza de Emergência (Hot Wipe) para Rastros USB e de Execução
# ATENÇÃO: Execute este script como ADMINISTRADOR. Ele fará alterações irreversíveis no sistema.
# Use por sua conta e risco. Não garante 100% de invisibilidade contra análise forense avançada.
# Este script foi modificado para suprimir ao máximo qualquer saída ou log próprio.

# Desativar a transcrição do PowerShell para evitar logs de execução do script
# Isso pode não ser 100% eficaz dependendo da configuração de GPO do sistema.
Set-PSReadLineOption -HistorySaveStyle SaveNothing -ErrorAction SilentlyContinue
Set-PSReadLineOption -AddToHistory $false -ErrorAction SilentlyContinue

# 1. Limpeza de Chaves de Registro USB (USBSTOR e USB)
#    Remoção agressiva de subchaves para apagar o histórico de dispositivos.
#    A remoção completa das chaves Enum\USBSTOR e Enum\USB pode causar instabilidade.
#    Focamos em subchaves que identificam dispositivos específicos.

$usbStorPath = "HKLM:\SYSTEM\CurrentControlSet\Enum\USBSTOR"
$usbPath = "HKLM:\SYSTEM\CurrentControlSet\Enum\USB"

# Remover subchaves de dispositivos USBSTOR (armazenamento)
try {
    Get-Item -Path $usbStorPath -ErrorAction SilentlyContinue | ForEach-Object {
        foreach ($subKey in $_.GetSubKeyNames()) {
            $fullSubKeyPath = "$usbStorPath\$subKey"
            Remove-Item -Path $fullSubKeyPath -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
        }
    }
} catch { }

# Remover subchaves de dispositivos USB (genéricos)
try {
    Get-Item -Path $usbPath -ErrorAction SilentlyContinue | ForEach-Object {
        foreach ($subKey in $_.GetSubKeyNames()) {
            $fullSubKeyPath = "$usbPath\$subKey"
            Remove-Item -Path $fullSubKeyPath -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
        }
    }
} catch { }

# Limpar chaves de MountedDevices (atribuição de letras de drive)
$mountedDevicesPath = "HKLM:\SYSTEM\MountedDevices"
try {
    Get-ItemProperty -Path $mountedDevicesPath -ErrorAction SilentlyContinue | ForEach-Object {
        $_.PSObject.Properties | ForEach-Object {
            if ($_.Name -like "\\??\\Volume{*" -or $_.Name -like "\\??\\USBSTOR#*" -or $_.Name -like "\\??\\_??_USBSTOR#*") {
                Remove-ItemProperty -LiteralPath $mountedDevicesPath -Name $_.Name -ErrorAction SilentlyContinue | Out-Null
            }
        }
    }
} catch { }

# 2. Limpeza do setupapi.dev.log
#    Tenta zerar o conteúdo do log. Pode falhar se o arquivo estiver em uso.
$setupapiLog = "$env:SystemRoot\inf\setupapi.dev.log"
try {
    Clear-Content -Path $setupapiLog -ErrorAction SilentlyContinue | Out-Null
} catch { }

# 3. Limpeza da pasta Prefetch
#    Remove todos os arquivos .pf. Isso pode ser um indicador de limpeza.
$prefetchPath = "$env:SystemRoot\Prefetch"
try {
    Get-ChildItem -Path $prefetchPath -Filter "*.pf" -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue | Out-Null
} catch { }

# 4. Limpeza de Arquivos Recentes (.lnk)
#    Remove atalhos de arquivos recentes.
$recentPath = "$env:AppData\Microsoft\Windows\Recent"
try {
    Get-ChildItem -Path $recentPath -Filter "*.lnk" -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue | Out-Null
} catch { }

# 5. Limpeza de Event Logs relacionados a USB/Armazenamento
#    Limpa logs de eventos que podem conter rastros de conexão USB.
$eventLogsToClear = @(
    "Microsoft-Windows-Partition/Diagnostic",
    "Microsoft-Windows-Storsvc/Operational",
    "Microsoft-Windows-Kernel-PnP/Configuration",
    "System",
    "Security" # Opcional, pode ser muito agressivo e levantar suspeitas
)

foreach ($log in $eventLogsToClear) {
    try {
        wevtutil cl $log -ErrorAction SilentlyContinue | Out-Null
    } catch { }
}

# 6. Tentativa de Limpeza de Shimcache (sem reboot, limitada)
#    A limpeza completa do Shimcache sem reboot é extremamente difícil e geralmente requer ferramentas especializadas.
#    A remoção do valor AppCompatCache pode invalidar o cache, mas o LastWrite time da chave ainda permanece.
$shimcachePath = "HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\AppCompatCache"
try {
    Remove-ItemProperty -LiteralPath $shimcachePath -Name "AppCompatCache" -ErrorAction SilentlyContinue | Out-Null
} catch { }

# 7. Limpeza de UserAssist (se possível, sem ferramentas externas)
#    UserAssist é difícil de limpar sem ferramentas ou reboot. A chave é ofuscada.
#    A remoção direta pode causar problemas ou ser detectada. Melhor deixar para ferramentas especializadas.

# 8. Limpeza de Amcache (sem reboot, extremamente difícil)
#    Amcache.hve é um arquivo de banco de dados e é muito difícil de limpar sem reboot ou ferramentas específicas.
#    A modificação direta pode corromper o arquivo e levantar suspeitas.

# Suprimir qualquer saída final do script
exit 0
```
