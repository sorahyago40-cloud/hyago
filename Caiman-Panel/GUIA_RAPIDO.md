# ⚡ GUIA RÁPIDO - CAIMAN PAINEL

## 🎯 Começar em 5 Minutos

### 1. Abrir Projeto
```bash
# Navegar para pasta
cd Caiman-Panel

# Abrir no Visual Studio
start Caiman.sln
```

### 2. Configurar KeyAuth
Edite `Caiman Panel/Form1.cs`:
```csharp
public static api KeyAuthApp = new api(
    name: "seu_app_name",          // ← MUDE ISSO
    ownerid: "seu_owner_id",       // ← MUDE ISSO
    secret: "seu_secret_key",      // ← MUDE ISSO
    version: "1.0.0"               // ← MUDE ISSO
);
```

### 3. Compilar
```
Build → Build Solution (F7)
```

### 4. Executar
```
Debug → Start (F5)
```

---

## 🎨 Customizar Cores

### Verde Neon CAIMAN:
```csharp
// Cores principais
#1FFF00  // Verde Neon (principal)
#00FF00  // Verde Neon (puro)
#1A1A1A  // Fundo (claro)
#0A0F0A  // Fundo (escuro)
#FFFFFF  // Texto branco
```

### Onde mudar:
1. `Form1.Designer.cs` - Tela de login
2. `Form2.Designer.cs` - Painel principal

### Exemplo:
```csharp
// Botão
guna2Button.FillColor = Color.FromArgb(31, 255, 0);     // Verde neon
guna2Button.HoverState.FillColor = Color.FromArgb(0, 255, 0);  // Verde mais brilhante

// Fundo
this.BackColor = Color.FromArgb(26, 26, 26);           // Fundo claro

// Texto
guna2TextBox.ForeColor = Color.FromArgb(255, 255, 255);  // Branco
```

---

## 🔑 Constantes Importantes

### Offsets Free Fire:
```csharp
// Localização no arquivo Form2.cs (linha ~38)
const long OFFSET_HEAD = 0xAE;      // Cabeça
const long OFFSET_CHEST = 0xEA;     // Peito
const long OFFSET_AIM = 0xB2;       // Mira
```

### Delay do Aimbot:
```csharp
// Linha ~43
private int delayEntreTiros = 100;  // Millisegundos (80-150 recomendado)
```

---

## 📂 Arquivos Principais

| Arquivo | Função |
|---------|--------|
| `Program.cs` | Inicializa app |
| `Form1.cs` | Tela de login |
| `Form2.cs` | Painel principal |
| `KeyAuth.cs` | Autenticação |
| `BastabCheatsMem.cs` | Engine de cheats |
| `Structs.cs` | Estruturas de dados |

---

## 🚀 Funcionalidades Principais

### ✅ Login
- Usuário/Senha via KeyAuth
- Salvamento automático
- Auto-login (comentado por padrão)

### ✅ Painel
- Interface Guna UI (moderna)
- Paleta verde neon
- Logo CAIMAN integrado
- Controles animados

### ✅ Aimbot
- Mira automática
- Delay configurável
- Offsets ajustáveis
- Suporta múltiplos alvos

### ✅ Memory
- Manipulação de memória
- Pattern scanning
- Thread remota
- Injector de DLL

---

## 🐛 Erros Comuns

### ❌ "Memory.dll not found"
```
Solução:
1. Pasta: Caiman Panel/
2. Criar: libs/
3. Copiar: Memory.dll para libs/
4. .csproj: Ajustar HintPath
```

### ❌ "Access Denied"
```
Solução:
- Execute como ADMINISTRADOR
- Ou execute o .exe com privilégios
```

### ❌ "KeyAuth init failed"
```
Solução:
- Verifique as credenciais em Form1.cs
- Teste conexão de internet
- Verifique se servidor KeyAuth está online
```

### ❌ "Pattern not found"
```
Solução:
- Offsets podem ter mudado
- Atualize o padrão hexadecimal
- Verifique versão do jogo
```

---

## 🔒 Segurança

### ⚠️ IMPORTANTE:

1. **Não distribuir credenciais**
   ```csharp
   // ❌ NÃO coloque em GitHub
   secret: "sua_chave_secreta"
   
   // ✅ Faça:
   // Use arquivo de config local
   // Use variáveis de ambiente
   ```

2. **Ofuscar antes de distribuir**
   ```
   Ferramentas recomendadas:
   - ConfuserEx
   - .NET Reactor
   - Eziriz NetGuard
   ```

3. **Teste em máquina segura**
   ```
   - VM isolada
   - Sem dados pessoais
   - Pronta para ser descartada
   ```

---

## 📊 Referência Rápida de Código

### Ativar Aimbot:
```csharp
// Form2.cs, método ButtonAimbot_Click
private void ButtonAimbot_Click(object sender, EventArgs e)
{
    if (!aimbotAtivado)
    {
        aimbotAtivado = true;
        aimbotCts = new CancellationTokenSource();
        StartAimbotLoop(aimbotCts.Token);
    }
    else
    {
        aimbotAtivado = false;
        aimbotCts?.Cancel();
    }
}
```

### Loop do Aimbot:
```csharp
private async Task StartAimbotLoop(CancellationToken ct)
{
    while (!ct.IsCancellationRequested)
    {
        try
        {
            // 1. Encontrar inimigo
            var enemy = FindNearestEnemy();
            
            // 2. Calcular ângulo
            var angle = CalculateAimAngle(enemy);
            
            // 3. Escrever na memória
            WriteAimToMemory(angle);
            
            // 4. Aguardar delay
            await Task.Delay(delayEntreTiros, ct);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro: " + ex.Message);
            break;
        }
    }
}
```

### Escrever na Memória:
```csharp
private void WriteAimToMemory(float angle)
{
    IntPtr processHandle = OpenProcess(
        PROCESS_VM_WRITE | PROCESS_VM_READ,
        false,
        targetProcessId
    );
    
    byte[] data = BitConverter.GetBytes(angle);
    WriteProcessMemory(processHandle, baseAddress + OFFSET_AIM, data, 
                      (uint)data.Length, out _);
}
```

---

## 📞 Suporte Rápido

### Documentação Completa:
- `README.md` - Guia completo
- `ANALISE_CODIGO.md` - Análise técnica
- Este arquivo - Referência rápida

### Passos para Debug:
1. Ativar "Debug" em Build
2. F5 para debug
3. Breakpoints (F9)
4. Ver variáveis (Debug → Windows → Variables)
5. Step over (F10) / Step into (F11)

---

## ✨ Próximos Passos

- [ ] Configurar KeyAuth
- [ ] Compilar Release
- [ ] Testar em VM
- [ ] Ofuscar código
- [ ] Customizar cores
- [ ] Adicionar logo
- [ ] Testar funcionalidades
- [ ] Documentar offsets

---

**Rápido e Fácil - CAIMAN Ready!** 🐊
