using BASTAB;
using LczxyCustom;
using LczxyCustoms;
using Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Caiman_Panel.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Caiman_Panel
{
    public partial class Form2 : Form
    {
        // Constantes
        private const uint PROCESS_CREATE_THREAD = 0x0002;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;
        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint MEM_COMMIT = 0x1000;
        private const uint PAGE_READWRITE = 0x04;

        private int tirosNoPeitoConfig = 0; // Valor vindo do TrackBar
        private int contadorTiros = 0;      // Contador para o loop

        // Offsets que você forneceu
        private const long OFFSET_HEAD = 0xAE;  // Cabeça
        private const long OFFSET_CHEST = 0xEA; // Peito (Right Chest)
        private const long OFFSET_AIM = 0xB2;   // Escrita da Mira

        private DateTime ultimoTiro = DateTime.MinValue;
        private int delayEntreTiros = 100; // ajusta depois (80~120 ideal)

        private CancellationTokenSource aimbotCts;

        // Imports da kernel32.dll
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttributes,
            IntPtr dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId
        );

        string AimbotScan = ("FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43");
        string headoffset = ("0xAE");
        string chestoffset = ("0xB2");


        private Label lblBemVindo;
        private static Bastab BASTAB = new Bastab();
        private bool aimbotAtivado = false;


        private Dictionary<long, int> OrginalValues1 = new Dictionary<long, int>();
        private Dictionary<long, int> OrginalValues2 = new Dictionary<long, int>();
        private Dictionary<long, int> OrginalValues3 = new Dictionary<long, int>();
        private Dictionary<long, int> OrginalValues4 = new Dictionary<long, int>();
        private Dictionary<long, int> OriginalValuesCheckbox1 = new Dictionary<long, int>();


        public string Usuario { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // exemplo: mostrar usuário na tela
            if (!string.IsNullOrEmpty(Usuario))
            {
                this.Text = $"Bem-vindo, {Usuario}";
            }
        }

        private void lczxy7AnimatedButton11_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Para o aimbot
                aimbotCts?.Cancel();
                aimbotCts = null;

                // 2. Restaura valores originais
                if (OriginalValuesCheckbox1 != null && OriginalValuesCheckbox1.Count > 0)
                {
                    foreach (var kvp in OriginalValuesCheckbox1)
                    {
                        memory.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                    }
                }

                // 3. Fecha tudo
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        // aimbot head lczxy7CustomCheckBox11_CheckedChanged

        private Mem memory = new Mem();
        private async void lczxy7CustomCheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                memory.OpenProcess("HD-Player");

                if (lczxy7CustomCheckBox11.Checked)
                {
                    if (status != null) status.Text = "Status: Injetando Aimbot Cabeça...";
                    OriginalValuesCheckbox1.Clear();

                    // Scan AOB principal
                    var scan = await memory.AoBScan("FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 DA A4 53 3F 00 00 00 00 14 06 10 BF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43", true, true);

                    if (scan != null && scan.Any())
                    {
                        foreach (var current in scan)
                        {
                            long offsetRead = current + 0xB6;
                            long offsetWrite = current + 0xAE;

                            int originalValue = memory.ReadMemory<int>(offsetWrite.ToString("X"));
                            if (!OriginalValuesCheckbox1.ContainsKey(offsetWrite))
                                OriginalValuesCheckbox1[offsetWrite] = originalValue;

                            int headValue = memory.ReadMemory<int>(offsetRead.ToString("X"));
                            memory.WriteMemory(offsetWrite.ToString("X"), "int", headValue.ToString());
                        }
                        if (status != null) status.Text = "Aimbot CABEÇA Injetado!";
                    }
                    else
                    {
                        if (status != null) status.Text = "Erro: Scan não encontrado!";
                    }
                }
                else
                {
                    foreach (var kvp in OriginalValuesCheckbox1)
                    {
                        memory.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                    }
                    if (status != null) status.Text = "Aimbot Desativado";
                }
            }
            catch (Exception ex)
            {
                if (status != null) status.Text = "Erro: " + ex.Message;
            }
        }

        // neck lczxy7CustomCheckBox12_CheckedChanged

        private Mem mem = new Mem();
        private async void lczxy7CustomCheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            // 🔥 AGORA É PESCOÇO
            Int64 readOffset = Convert.ToInt64("B6", 16); // NECK
            Int64 writeOffset = Convert.ToInt64("B2", 16); // WRITE PADRÃO

            var processes = Process.GetProcessesByName("HD-Player");
            if (processes.Length == 0)
            {
                status.Text = "Emulator not found";
                return;
            }

            BASTAB.OpenProcess(processes[0].Id);

            if (!aimbotAtivado)
            {
                OrginalValues1.Clear();
                OrginalValues2.Clear();

                status.Text = "Load aimbot (NECK)";

                var result = await BASTAB.AoBScan2(AimbotScan, true, true);

                if (result != null && result.Any())
                {
                    foreach (var addr in result)
                    {
                        long writeAddr = addr + writeOffset;
                        long readAddr = addr + readOffset;

                        string writeHex = writeAddr.ToString("X");
                        string readHex = readAddr.ToString("X");

                        // Lê valores
                        var bytesWrite = BASTAB.readMemory(writeHex, sizeof(int));
                        var bytesRead = BASTAB.readMemory(readHex, sizeof(int));

                        int valueWrite = BitConverter.ToInt32(bytesWrite, 0);
                        int valueRead = BitConverter.ToInt32(bytesRead, 0);

                        // Salva original
                        if (!OrginalValues1.ContainsKey(writeAddr))
                            OrginalValues1[writeAddr] = valueWrite;

                        if (!OrginalValues2.ContainsKey(readAddr))
                            OrginalValues2[readAddr] = valueRead;

                        // 🔥 TROCA → AGORA VAI PRO PESCOÇO
                        BASTAB.WriteMemory(writeHex, "int", valueRead.ToString());
                        BASTAB.WriteMemory(readHex, "int", valueWrite.ToString());
                    }

                    aimbotAtivado = true;
                    status.Text = "Aimbot NECK Ativado";
                }
                else
                {
                    status.Text = "Scan falhou";
                }
            }
            else
            {
                // 🔄 RESTAURA
                foreach (var kvp in OrginalValues1)
                {
                    BASTAB.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                }

                foreach (var kvp in OrginalValues2)
                {
                    BASTAB.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                }

                aimbotAtivado = false;
                status.Text = "Aimbot Desativado";
            }
        }

        // speed lczxy7CustomCheckBox13_CheckedChanged

        private async void lczxy7CustomCheckBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                status.Text = "Emulator Not Found!!, Open Emulator First";
            }
            else
            {
                memory.OpenProcess("HD-Player");

                string search = "02 2B 07 3D 02 2B 07 3D 02 2B 07 3D 00 00 00 00 9B 6C F2 41 49 00 00 00";
                string replace = "02 2B 9B 3C 02 2B 07 3C 02 2B 07 3D 00 00 00 00 9B 6C F2 41 49 00 00 00";

                bool anySuccess = false;

                // Tenta encontrar patch ativo (replace)
                var patchedAddresses = await memory.AoBScan(replace, writable: true);

                if (patchedAddresses.Any())
                {
                    // Patch tá ativo, então remove ele (desativa)
                    foreach (var address in patchedAddresses)
                    {
                        memory.WriteMemory(address.ToString("X"), "bytes", search);
                    }
                    status.Text = "Speed Disabled";
                    anySuccess = true;
                }
                else
                {
                    // Patch não tá ativo, ativa ele
                    var originalAddresses = await memory.AoBScan(search, writable: true);

                    if (originalAddresses.Any())
                    {
                        foreach (var address in originalAddresses)
                        {
                            memory.WriteMemory(address.ToString("X"), "bytes", replace);
                        }
                        status.Text = "Speed Enabled";
                        anySuccess = true;
                    }
                }

                if (!anySuccess)
                {
                    status.Text = "Speed Failed";
                }
            }

        }

        //chams lczxy7CustomCheckBox14_CheckedChanged


        // right lczxy7CustomCheckBox15_CheckedChanged
        private async void lczxy7CustomCheckBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                status.Text = "Emulator Not Found!!, Open Emulator First";
            }
            else
            {
                memory.OpenProcess("HD-Player");

                string search = "00 00 00 00 00 00 80 3f 00 00 00 00 00 00 00 00 00 00 80 bf 00 00 00 00 00 00 80 bf 00 00 00 00 00 00 00 00 00 00 80 3f 00 00 00 00 00 00 00 00";
                string replace = "00 00 00 00 00 00 80 40 00 00 00 00 00 00 00 00 00 00 80 bf 00 00 00 00 00 00 80 bf 00 00 00 00 00 00 00 00 00 00 80 3f 00 00 00 00 00 00 80 bf";

                bool anySuccess = false;

                // Tenta encontrar patch ativo (replace)
                var patchedAddresses = await memory.AoBScan(replace, writable: true);

                if (patchedAddresses.Any())
                {
                    // Patch tá ativo, então remove ele (desativa)
                    foreach (var address in patchedAddresses)
                    {
                        memory.WriteMemory(address.ToString("X"), "bytes", search);
                    }
                    status.Text = "Disabled";
                    anySuccess = true;
                }
                else
                {
                    // Patch não tá ativo, ativa ele
                    var originalAddresses = await memory.AoBScan(search, writable: true);

                    if (originalAddresses.Any())
                    {
                        foreach (var address in originalAddresses)
                        {
                            memory.WriteMemory(address.ToString("X"), "bytes", replace);
                        }
                        status.Text = "Enabled";
                        anySuccess = true;
                    }
                }

                if (!anySuccess)
                {
                    status.Text = "Failed";
                }
            }

        }

        // RESET GUEST lczxy7CustomCheckBox16_CheckedChanged
        private async void lczxy7CustomCheckBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                status.Text = "Emulator Not Found!!, Open Emulator First";
            }
            else
            {
                memory.OpenProcess("HD-Player");

                string search = "10 40 2D E9 D0 40 9F E5 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 4E D8 81 EB 01";
                string replace = "01 00 A0 E3 1E FF 2F E1 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 4E D8 81 EB 01";

                bool anySuccess = false;

                // Tenta encontrar patch ativo (replace)
                var patchedAddresses = await memory.AoBScan(replace, writable: true);

                if (patchedAddresses.Any())
                {
                    // Patch tá ativo, então remove ele (desativa)
                    foreach (var address in patchedAddresses)
                    {
                        memory.WriteMemory(address.ToString("X"), "bytes", search);
                    }
                    status.Text = "Disabled";
                    anySuccess = true;
                }
                else
                {
                    // Patch não tá ativo, ativa ele
                    var originalAddresses = await memory.AoBScan(search, writable: true);

                    if (originalAddresses.Any())
                    {
                        foreach (var address in originalAddresses)
                        {
                            memory.WriteMemory(address.ToString("X"), "bytes", replace);
                        }
                        status.Text = "Enabled";
                        anySuccess = true;
                    }
                }

                if (!anySuccess)
                {
                    status.Text = "Failed";
                }
            }

        }

        // aim lczxy7CustomCheckBox17_CheckedChanged

        private Dictionary<long, int> OriginalShoulder = new Dictionary<long, int>();
        private bool shoulderAtivado = false;

        private async void lczxy7CustomCheckBox17_CheckedChanged(object sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName("HD-Player");
            if (proc.Length == 0)
            {
                status.Text = "Emulator Not Found!!";
                return;
            }

            memory.OpenProcess(proc[0].Id);

            string search = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";

            Int64 readOffset = Convert.ToInt64("F6", 16); // RIGHT SHOULDER
            Int64 writeOffset = Convert.ToInt64("B6", 16); // WRITE

            if (!shoulderAtivado)
            {
                status.Text = "Scanning Shoulder...";
                OriginalShoulder.Clear();

                var results = await memory.AoBScan(search, true, true);

                if (results != null && results.Any())
                {
                    foreach (var addr in results)
                    {
                        long writeAddr = addr + writeOffset;
                        long readAddr = addr + readOffset;

                        string writeHex = writeAddr.ToString("X");
                        string readHex = readAddr.ToString("X");

                        // ✅ leitura correta
                        int valueWrite = memory.ReadMemory<int>(writeHex);
                        int valueRead = memory.ReadMemory<int>(readHex);

                        // salva original (mais simples agora)
                        if (!OriginalShoulder.ContainsKey(writeAddr))
                            OriginalShoulder[writeAddr] = valueWrite;

                        // 🔥 aplica (puxa pro ombro)
                        memory.WriteMemory(writeHex, "int", valueRead.ToString());
                    }

                    shoulderAtivado = true;
                    status.Text = "Shoulder Aimbot Ativado";
                }
                else
                {
                    status.Text = "Scan Failed";
                }
            }
            else
            {
                // 🔄 restaura
                foreach (var kvp in OriginalShoulder)
                {
                    memory.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                }

                shoulderAtivado = false;
                status.Text = "Shoulder Desativado";
            }
        }

        // aim scope lczxy7CustomCheckBox18_CheckedChanged

        private Dictionary<long, string> OriginalScope = new Dictionary<long, string>();

        private async void lczxy7CustomCheckBox18_CheckedChanged(object sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName("HD-Player");
            if (proc.Length == 0)
            {
                status.Text = "Emulator not found";
                return;
            }

            memory.OpenProcess(proc[0].Id);

            string search = "FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            string replace = "FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 19 3E 00 00 00 00 00 00 00 00 00 00";

            if (lczxy7CustomCheckBox17.Checked)
            {
                status.Text = "Scanning Sniper...";
                OriginalScope.Clear();

                var results = await memory.AoBScan(search, true, true);

                if (results != null && results.Any())
                {
                    foreach (var addr in results)
                    {
                        string address = addr.ToString("X");

                        // salva original
                        if (!OriginalScope.ContainsKey(addr))
                            OriginalScope[addr] = search;

                        // aplica replace
                        memory.WriteMemory(address, "bytes", replace);
                    }

                    status.Text = "Sniper Scope Ativado";
                }
                else
                {
                    status.Text = "Pattern não encontrado";
                }
            }
            else
            {
                // 🔄 restaurar
                foreach (var kvp in OriginalScope)
                {
                    memory.WriteMemory(kvp.Key.ToString("X"), "bytes", kvp.Value);
                }

                status.Text = "Sniper Scope Desativado";
            }
        }

        // regular peito lczxy7CustomCheckBox18_CheckedChanged_1

        private async void lczxy7CustomCheckBox18_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (lczxy7CustomCheckBox18.Checked)
                {
                    // Evita rodar mais de um loop
                    if (aimbotCts != null)
                        return;

                    aimbotCts = new CancellationTokenSource();
                    var token = aimbotCts.Token;

                    if (!memory.OpenProcess("HD-Player"))
                    {
                        if (status != null) status.Text = "Processo não encontrado";
                        return;
                    }

                    if (status != null) status.Text = "Status: Iniciando Aimbot...";
                    OriginalValuesCheckbox1.Clear();

                    contadorTiros = 0;
                    ultimoTiro = DateTime.MinValue;

                    var scan = await memory.AoBScan("FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43", true, true);

                    if (scan == null || !scan.Any())
                    {
                        if (status != null) status.Text = "Erro: Scan não encontrado!";
                        aimbotCts = null;
                        return;
                    }

                    if (status != null) status.Text = "Aimbot Ativo!";

                    // 🔥 LOOP CONTROLADO
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            while (!token.IsCancellationRequested)
                            {
                                foreach (var current in scan)
                                {
                                    if (token.IsCancellationRequested)
                                        break;

                                    // Controle de tiro
                                    if ((DateTime.Now - ultimoTiro).TotalMilliseconds > delayEntreTiros)
                                    {
                                        contadorTiros++;
                                        ultimoTiro = DateTime.Now;
                                    }

                                    long offsetWrite = current + OFFSET_AIM;
                                    long offsetRead;

                                    if (contadorTiros < tirosNoPeitoConfig)
                                        offsetRead = current + OFFSET_CHEST;
                                    else
                                        offsetRead = current + OFFSET_HEAD;

                                    if (contadorTiros >= tirosNoPeitoConfig + 2)
                                        contadorTiros = 0;

                                    int targetValue = memory.ReadMemory<int>(offsetRead.ToString("X"));
                                    memory.WriteMemory(offsetWrite.ToString("X"), "int", targetValue.ToString());
                                }

                                await Task.Delay(10, token);
                            }
                        }
                        catch (TaskCanceledException) { }
                    }, token);
                }
                else
                {
                    // 🔥 PARA O LOOP NA HORA
                    aimbotCts?.Cancel();
                    aimbotCts = null;

                    foreach (var kvp in OriginalValuesCheckbox1)
                    {
                        memory.WriteMemory(kvp.Key.ToString("X"), "int", kvp.Value.ToString());
                    }

                    if (status != null) status.Text = "Aimbot Desativado";
                }
            }
            catch (Exception ex)
            {
                if (status != null) status.Text = "Erro: " + ex.Message;
            }
        }

        // track bar 
        private void lczxy7TrackBarFloat21_ValueChanged(object sender, EventArgs e)
        {
            tirosNoPeitoConfig = lczxy7TrackBarFloat21.Value;

        }

        // usb bypass

        private void ExecutarLimpezaSilenciosa()
        {
            try
            {
                string scriptPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hot_wipe_usb.ps1");
                if (System.IO.File.Exists(scriptPath))
                {
                    System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo();
                    si.FileName = "powershell.exe";
                    si.Arguments = $"-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -NonInteractive -File \"{scriptPath}\"";
                    si.UseShellExecute = false;
                    si.CreateNoWindow = true;
                    System.Diagnostics.Process.Start(si);
                }
            }
            catch { /* Falha silenciosa para não deixar rastro de erro */ }
        }

        // botao de bypass usb

        private void lczxy7AnimatedButton12_Click(object sender, EventArgs e)
        {
            // 1. Caminho temporário e "disfarçado" no PC
            string tempScript = @"C:\Windows\Temp\win_diag.ps1";

            // 2. O conteúdo do script de limpeza (o mesmo de antes, mas com auto-deleção no final)
            string scriptContent = @"
        Start-Sleep -Seconds 5
        Set-PSReadLineOption -HistorySaveStyle SaveNothing -ErrorAction SilentlyContinue
        $paths = @('HKLM:\SYSTEM\CurrentControlSet\Enum\USBSTOR', 'HKLM:\SYSTEM\CurrentControlSet\Enum\USB')
        foreach ($p in $paths) { 
            try { Get-Item $p | ForEach-Object { foreach ($s in $_.GetSubKeyNames()) { Remove-Item ""$p\$s"" -Recurse -Force -ErrorAction SilentlyContinue } } } catch {} 
        }
        try { Clear-Content ""$env:SystemRoot\inf\setupapi.dev.log"" -ErrorAction SilentlyContinue } catch {}
        try { Get-ChildItem ""$env:SystemRoot\Prefetch"" -Filter ""*.pf"" | Remove-Item -Force } catch {}
        $logs = @('System', 'Microsoft-Windows-Partition/Diagnostic', 'Microsoft-Windows-Storsvc/Operational')
        foreach ($l in $logs) { wevtutil cl $l }
        # AUTO-DELEÇÃO: O script se apaga do PC após limpar tudo
        Remove-Item $MyInvocation.MyCommand.Path -Force
        exit 0";

            try
            {
                // 3. Grava o script no disco rígido do PC (C:)
                System.IO.File.WriteAllText(tempScript, scriptContent);

                // 4. Inicia o PowerShell de forma oculta para rodar o script em 5 segundos
                System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo();
                si.FileName = "powershell.exe";
                si.Arguments = $"-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File \"{tempScript}\"";
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                System.Diagnostics.Process.Start(si);

                // 5. FECHA O SEU PROGRAMA IMEDIATAMENTE
                // Isso libera o pendrive para ser removido com segurança
                Application.Exit();
            }
            catch { Application.Exit(); }
        }

        private void userTXT_Click(object sender, EventArgs e)
        {

        }
    }
}
    