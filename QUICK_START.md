# 🐊 CAIMAN - INÍCIO RÁPIDO (QUICK START)

## 🇧🇷 PORTUGUÊS (Versão Rápida - 10 Minutos)

### ⚡ Pré-requisito
- Node.js 18+ 
- .NET 8 SDK
- MongoDB (local ou Docker)

### 🚀 Backend (5 minutos)

```bash
cd Caiman-Backend
npm install
npm start
```

✅ Se vir "CAIMAN Backend running on port 3000" = sucesso!

### 📱 App Mobile (5 minutos)

```bash
cd Caiman-Mobile
dotnet workload install maui
dotnet maui run -f net8.0-android
```

### ✅ Testar Login
- Username: `admin`
- Password: `admin123`

**Pronto! Tudo funcionando! 🎉**

---

## 🇬🇧 ENGLISH (Quick Version - 10 Minutes)

### ⚡ Prerequisites
- Node.js 18+
- .NET 8 SDK
- MongoDB (local or Docker)

### 🚀 Backend (5 minutes)

```bash
cd Caiman-Backend
npm install
npm start
```

✅ If you see "CAIMAN Backend running on port 3000" = success!

### 📱 Mobile App (5 minutes)

```bash
cd Caiman-Mobile
dotnet workload install maui
dotnet maui run -f net8.0-android
```

### ✅ Test Login
- Username: `admin`
- Password: `admin123`

**Done! Everything working! 🎉**

---

## 🐛 Problemas Comuns

| Problema | Solução |
|----------|---------|
| `❌ Cannot connect to MongoDB` | Instalar MongoDB ou usar Docker: `docker-compose up` |
| `❌ Port 3000 already in use` | Mudar PORT em `.env` ou matar processo: `lsof -i :3000` |
| `❌ App não conecta` | Editar `ApiService.cs` com URL correta |
| `❌ Workload maui not installed` | `dotnet workload install maui` |
| `❌ Android SDK not found` | `dotnet workload install android` |

---

## 📂 Arquivos Importantes

```
.
├── Caiman-Backend/          ← Servidor API
│   ├── start.sh/.bat        ← Script para iniciar
│   ├── .env                 ← Configurações
│   ├── README.md            ← Documentação completa
│   ├── routes/              ← Endpoints API
│   └── models/              ← Database schemas
│
├── Caiman-Mobile/           ← App Mobile
│   ├── Services/
│   │   └── ApiService.cs    ← Mudar URL aqui!
│   ├── Pages/               ← Telas
│   ├── ViewModels/          ← Lógica
│   └── README_MAUI.md       ← Documentação
│
├── SETUP_COMPLETO.md        ← Guia detalhado
└── QUICK_START.md           ← Este arquivo
```

---

## 🔧 Configurar Backend URL

**IMPORTANTE!** Editar `Caiman-Mobile/Services/ApiService.cs` linha ~13:

```csharp
// Para desenvolvimento LOCAL:
private const string BaseUrl = "http://10.0.2.2:3000"; // Android emulator
// OU
private const string BaseUrl = "http://SEU_IP_LOCAL:3000"; // Celular

// Para produção:
private const string BaseUrl = "https://seu-servidor.com:3000";
```

---

## 📡 Verificar Backend Online

```bash
# Terminal/PowerShell
curl http://localhost:3000/api/health

# Resposta esperada:
{
  "success": true,
  "status": "healthy",
  "message": "CAIMAN Backend is running"
}
```

---

## 🐳 Usar Docker (Alternativa)

```bash
cd Caiman-Backend
docker-compose up -d

# Verificar:
docker-compose ps

# Parar:
docker-compose down
```

---

## 📊 Arquitetura Básica

```
┌──────────────────┐
│  MAUI App        │
│  (Android/iOS)   │
└────────┬─────────┘
         │ HTTP
         ▼
┌──────────────────┐
│  Node.js Backend │
│  (API REST)      │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│  MongoDB         │
│  (Database)      │
└──────────────────┘
```

---

## ✅ Checklist

- [ ] Node.js 18+ instalado
- [ ] .NET 8 SDK instalado  
- [ ] MongoDB rodando
- [ ] Backend iniciado
- [ ] App compilado
- [ ] Login funcionando
- [ ] Painel exibindo

---

## 🆘 Precisa de Ajuda?

1. **Backend não inicia?**
   - Verificar MongoDB: `mongod --version`
   - Verificar logs: `npm run dev`

2. **App não conecta?**
   - Verificar URL em ApiService.cs
   - Verificar backend online: `curl http://localhost:3000/api/health`

3. **Login falha?**
   - Usuario: `admin`
   - Senha: `admin123`
   - Backend rodando? `npm start`

---

## 📚 Documentação Completa

- `SETUP_COMPLETO.md` - Guia detalhado (30min)
- `Caiman-Backend/README.md` - Documentação Backend
- `Caiman-Mobile/README_MAUI.md` - Documentação Mobile

---

## 🚀 Deploy (Depois)

1. **Servidor**: Heroku, Railway, Render
2. **Mobile**: Google Play, Apple App Store

Veja `SETUP_COMPLETO.md` para deploy instructions.

---

**Versão**: 1.0.0  
**Status**: 🟢 **100% Pronto**  

🐊 **CAIMAN - Simples. Rápido. Poderoso.** 🐊
