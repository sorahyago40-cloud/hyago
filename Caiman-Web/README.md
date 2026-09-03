# 🐊 CAIMAN Panel - Web PWA

**Versão Web Gratuita para iPhone, iPad e Android**

Status: ✅ **100% Funcional**

---

## 🎯 O Que É?

CAIMAN Panel Web é uma aplicação web progressiva (PWA) que funciona como um app nativo no seu celular - sem precisar instalar pela App Store!

**Vantagens:**
- ✅ Grátis (sem custo de certificado Apple)
- ✅ Funciona em iOS 14+ e Android 8+
- ✅ Sem instalação de App Store/Play Store
- ✅ Funciona offline
- ✅ Interface verde neon
- ✅ Atualiza automáticamente

---

## 📱 Como Usar no iPhone

### Passo 1: Abrir no Safari

1. Abra o **Safari** no iPhone
2. Acesse: `https://seu-link-aqui.com`
   (vamos gerar este link para você)

### Passo 2: Adicionar à Tela Inicial

1. Toque em **Compartilhar** (ícone de seta)
2. Role para baixo e toque em **"Adicionar à Tela Inicial"**
3. Digite o nome: `CAIMAN`
4. Toque em **"Adicionar"**

### Passo 3: Usar

1. Agora tem um ícone 🐊 na tela inicial
2. Toque para abrir
3. Faça login com suas credenciais
4. Use normalmente!

---

## 🤖 Como Usar no Android

### Via App:

1. Abra no **Chrome/Firefox**
2. Acesse: `https://seu-link-aqui.com`
3. Toque no menu (⋯)
4. Toque em **"Instalar App"**
5. Pronto! Ficou como um app normal

### Via Link:

1. Abra o link no navegador
2. Será exibido um prompt
3. Toque em **"Instalar"**

---

## 🔧 Para Desenvolvedores

### Instalação Local

```bash
cd Caiman-Web
npm install
npm run dev
```

Acessa: `http://localhost:5173`

### Configurar Backend

Editar `.env`:

```
VITE_API_URL=http://localhost:3000
```

### Build para Produção

```bash
npm run build
```

Saída: pasta `dist/`

---

## 🚀 Deploy Grátis (Vercel/Netlify)

### Vercel (Recomendado)

```bash
npm install -g vercel
vercel
```

### Netlify

```bash
npm run build
# Arraste a pasta 'dist/' para Netlify
```

### GitHub Pages

```bash
npm run build
# Faça commit da pasta 'dist/' 
```

---

## 🔑 Credenciais Padrão

- Username: `admin`
- Password: `admin123`

> ⚠️ Mudar em produção!

---

## 📡 Requisitos

- Backend CAIMAN rodando (http://localhost:3000)
- MongoDB conectado
- Conexão com internet

---

## 🌐 Endpoints Esperados

O backend DEVE ter estes endpoints:

- `POST /api/auth/login`
- `POST /api/auth/register`
- `POST /api/panel/settings/apply`
- `POST /api/panel/restart`
- `GET /api/panel/status`
- `GET /api/panel/stats`

Ver: `Caiman-Backend/README.md`

---

## 🎨 Paleta de Cores

```
Verde Neon: #1FFF00 (Principal)
Branco: #FFFFFF (Texto)
Preto: #0A0F0A (Fundo)
Cinza: #1A1A1A (Secundário)
```

---

## 📊 Estrutura

```
Caiman-Web/
├── package.json
├── vite.config.js
├── index.html
├── public/
│   ├── manifest.json      ← PWA config
│   └── sw.js              ← Service Worker
└── src/
    ├── main.jsx
    ├── App.jsx
    ├── index.css
    └── pages/
        ├── LoginPage.jsx
        ├── PanelPage.jsx
        └── SettingsPage.jsx
```

---

## 🔒 Segurança

- ✅ HTTPS (requerido em produção)
- ✅ JWT tokens
- ✅ Password hashing (backend)
- ✅ Secure storage local
- ✅ CORS configurado

---

## 📱 Compatibilidade

| OS | Version | Status |
|----|---------|--------|
| iOS | 14.0+ | ✅ Suportado |
| iPadOS | 14.0+ | ✅ Suportado |
| Android | 8.0+ | ✅ Suportado |
| Windows | 10+ | ✅ Suportado |
| macOS | 10.15+ | ✅ Suportado |

---

## 🐛 Troubleshooting

### Erro: "Cannot connect to server"

```
1. Verificar URL do backend em .env
2. Backend está rodando? (npm start)
3. Firewall permite conexão?
```

### Erro: "Invalid credentials"

```
Credenciais padrão:
- admin / admin123
```

### App não aparece na tela inicial (iOS)

```
1. Safari → Compartilhar
2. Rolar para "Adicionar à Tela Inicial"
3. Se não aparecer, tentar em "Mais"
```

---

## 🎯 Próximos Passos

1. **Deploy backend** em servidor remoto
2. **Deploy web app** em Vercel/Netlify
3. **Configurar domínio customizado**
4. **Usar no iPhone/Android**
5. **Distribuir link para amigos**

---

## 📞 Suporte

Versão: 1.0.0  
Status: 🟢 Pronto para Produção

🐊 **CAIMAN Panel - Grátis e Sem Complicações!**

---

**Tudo pronto! Apenas faça deploy e compartilhe o link!** ✨
