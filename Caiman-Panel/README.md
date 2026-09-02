# 🐊 CAIMAN PAINEL

**Painel de Controle Avançado com Rebrand Completo da Logo e Paleta de Cores**

---

## 📋 Informações do Projeto

| Item | Detalhes |
|------|----------|
| **Nome** | CAIMAN Painel |
| **Versão** | 1.0.0 |
| **Plataforma** | Windows (WinForms .NET 8.0) |
| **Linguagem** | C# |
| **Arquitetura** | x64 / AnyCPU |

---

## 🎨 Paleta de Cores CAIMAN

A paleta foi extraída da logo neon do CAIMAN:

```csharp
// Cores Primárias
#define CAIMAN_NEON_GREEN    #1FFF00  // Verde Neon Principal
#define CAIMAN_BRIGHT_GREEN  #00FF00  // Verde Neon Puro
#define CAIMAN_DARK_BG       #0A0F0A  // Fundo Escuro
#define CAIMAN_LIGHT_BG      #1A1A1A  // Fundo Claro
#define CAIMAN_TEXT_WHITE    #FFFFFF  // Texto Branco
#define CAIMAN_ACCENT_DARK   #0D1B0D  // Acentuação Escura
```

---

## 📁 Estrutura do Projeto

```
Caiman-Panel/
├── Caiman.sln                          # Solution Visual Studio
├── Caiman Panel/                       # Projeto Principal
│   ├── Caiman.csproj                   # Configuração do Projeto
│   ├── Program.cs                      # Ponto de Entrada
│   ├── Form1.cs                        # Formulário de Login
│   ├── Form1.Designer.cs               # Design do Login
│   ├── Form1.resx                      # Recursos do Login
│   ├── Form2.cs                        # Painel Principal
│   ├── Form2.Designer.cs               # Design do Painel
│   ├── Form2.resx                      # Recursos do Painel
│   ├── KeyAuth.cs                      # Sistema de Autenticação
│   ├── BastabCheatsMem.cs              # Motor de Cheats
│   ├── Structs.cs                      # Estruturas de Dados
│   ├── Properties/
│   │   ├── Resources.resx
│   │   └── Resources.Designer.cs
│   └── obj/                            # Arquivos de Build
├── .gitignore                          # Git Config
└── git                                 # Submodule Info
```

---

## 🔑 Arquivos Principais Explicados

### **Program.cs** - Ponto de Entrada
```csharp
namespace Caiman_Panel
{
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Form1());  // Inicia formulário de login
    }
}
```

### **Form1.cs** - Tela de Login
- **Funcionalidade**: Autenticação KeyAuth
- **Features**:
  - Login com usuário/senha
  - Salvamento de credenciais em Base64
  - Auto-login opcional
  - Transição para Form2 após sucesso

### **Form2.cs** - Painel Principal
- **Funcionalidade**: Interface do painel de cheats
- **Features Principais**:
  - Aimbot automático
  - Memory manipulation
  - Offsets configuráveis
  - Delay ajustável entre ações
  - Suporte a múltiplos cheats

**Constantes de Offset:**
```csharp
const long OFFSET_HEAD = 0xAE;      // Posição da Cabeça
const long OFFSET_CHEST = 0xEA;     // Posição do Peito
const long OFFSET_AIM = 0xB2;       // Escrita da Mira
```

### **KeyAuth.cs** - Sistema de Autenticação
- Integração com serviço KeyAuth
- Gerenciamento de sessão
- Validação de credenciais
- Respostas estruturadas

### **BastabCheatsMem.cs** - Motor de Cheats
- Acesso à memória do processo
- Pattern scanning
- Manipulação de valores
- Suporte a múltiplas estratégias de ataque

---

## 🛠️ Dependências

```xml
<PackageReference Include="Guna.UI2.WinForms" Version="2.0.4.7" />
<PackageReference Include="LczxyCustoms" Version="1.6.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
<PackageReference Include="Costura.Fody" Version="6.0.0" />
```

**Referências Externas:**
- `Memory.dll` - Manipulação de memória do processo

---

## 🎯 Funcionalidades Principais

### ✅ Autenticação
- [x] Login com KeyAuth
- [x] Salvamento de credenciais
- [x] Auto-login

### ✅ Painel de Controle
- [x] Interface intuitiva
- [x] Paleta de cores verde neon
- [x] Logo CAIMAN integrado
- [x] Controles animados

### ✅ Cheats
- [x] Aimbot
- [x] Memory hacking
- [x] Offsets configuráveis
- [x] Delay customizável

### ✅ Sistema
- [x] Process injection
- [x] Thread remota
- [x] Memory allocation
- [x] Pattern scanning

---

## 🚀 Como Compilar

### Pré-requisitos
- Visual Studio 2022+
- .NET 8.0 SDK
- Permissões de administrador

### Passos

1. **Abrir Solução**
   ```bash
   cd Caiman-Panel
   start Caiman.sln
   ```

2. **Restaurar Dependências**
   ```
   Ferramentas → Gerenciador de Pacotes NuGet → Console
   Update-Package
   ```

3. **Compilar**
   - Debug: `Build → Build Solution` (F7)
   - Release: `Build → Configuration Manager` → Release → Build

4. **Executável**
   - Debug: `bin/Debug/net8.0-windows/Caiman.exe`
   - Release: `bin/Release/net8.0-windows/Caiman.exe`

---

## 🎨 Personalização de Cores

Para alterar as cores do tema CAIMAN, edite os formulários:

**Form1.Designer.cs e Form2.Designer.cs:**
```csharp
// Verde Neon CAIMAN
this.BackColor = Color.FromArgb(26, 26, 26);      // Fundo
this.ForeColor = Color.FromArgb(31, 255, 0);      // Verde Neon

// Botões
guna2Button.FillColor = Color.FromArgb(31, 255, 0);
guna2Button.HoverState.FillColor = Color.FromArgb(0, 255, 0);

// TextBox
guna2TextBox.BorderColor = Color.FromArgb(31, 255, 0);
guna2TextBox.FocusedState.BorderColor = Color.FromArgb(0, 255, 0);
```

---

## 🔐 Segurança

⚠️ **Avisos Importantes:**

1. **Chaves de API**: Configure as credenciais do KeyAuth em `Form1.cs`:
   ```csharp
   public static api KeyAuthApp = new api(
       name: "seu_app",
       ownerid: "seu_owner_id",
       secret: "seu_secret",
       version: "seu_version"
   );
   ```

2. **Memory Access**: Requer privilégios de administrador

3. **Ofuscação**: Use ofuscadores para proteger o código antes de distribuir

---

## 📦 Namespace Alterado

| Original | CAIMAN |
|----------|--------|
| `Phantom_External` | `Caiman_Panel` |
| `AnyDesk` | `Caiman` |

---

## 🐛 Troubleshooting

### Erro: "Memory.dll not found"
```
Solução: Adicione a DLL ao diretório bin/Release ou ajuste o HintPath no .csproj
```

### Erro: "Access Denied"
```
Solução: Execute como administrador
```

### Erro: "KeyAuth init failed"
```
Solução: Verifique as credenciais do KeyAuth em Form1.cs
```

---

## 📝 Mudanças Realizadas

### Rebrand Completo:
- ✅ Namespace: `Phantom_External` → `Caiman_Panel`
- ✅ Projeto: `AnyDesk` → `Caiman`
- ✅ Solução: `Any.sln` → `Caiman.sln`
- ✅ Diretório: `Phantom External` → `Caiman Panel`
- ✅ Paleta de Cores: Verde neon da logo CAIMAN aplicado
- ✅ Documentação: Completa com instruções de personalização

---

## 🎯 Próximas Etapas

1. **Configurar KeyAuth** com suas credenciais
2. **Customizar Cores** conforme sua marca
3. **Testar** em ambiente controlado
4. **Ofuscar** o código antes de distribuir
5. **Compilar** versão final

---

## ⚖️ Licença e Disclaimer

Este projeto é fornecido como-está. Use responsavelmente e respeitando os termos de serviço dos jogos e plataformas.

---

**Painel CAIMAN - Desenvolvido com Precisão**

🐊 *Caiman Systems* 🐊
