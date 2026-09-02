# 🐊 CAIMAN Backend API

Backend REST API para o painel CAIMAN mobile (Android/iOS).

**Status**: ✅ **Pronto para Produção**

---

## 📋 Requisitos

- **Node.js** 18+ (LTS)
- **MongoDB** 5.0+ (local ou cloud)
- **npm** 9+

---

## 🚀 Instalação Rápida (5 minutos)

### Opção 1: Local com MongoDB Local

#### Passo 1: Instalar MongoDB

**Windows:**
```bash
# Baixar e instalar do: https://www.mongodb.com/try/download/community
# Ou usar Chocolatey:
choco install mongodb-community
```

**macOS:**
```bash
brew tap mongodb/brew
brew install mongodb-community
brew services start mongodb-community
```

**Linux (Ubuntu/Debian):**
```bash
sudo apt-get update
sudo apt-get install -y mongodb
sudo systemctl start mongodb
```

#### Passo 2: Clonar e Instalar Dependências

```bash
cd Caiman-Backend
npm install
```

#### Passo 3: Configurar Variáveis de Ambiente

```bash
cp .env.example .env
# Editar .env com valores desejados (padrão: localhost:27017)
```

#### Passo 4: Executar Backend

```bash
npm start
# Ou em desenvolvimento:
npm run dev
```

Acesso: `http://localhost:3000`

---

### Opção 2: Docker (Recomendado)

Requer **Docker** e **Docker Compose**.

```bash
# Iniciar MongoDB + Backend com um comando
docker-compose up -d

# Verificar status
docker-compose ps

# Logs
docker-compose logs -f backend
```

Acesso: `http://localhost:3000`

Parar:
```bash
docker-compose down
```

---

### Opção 3: MongoDB Cloud (Atlas)

#### Passo 1: Criar Conta MongoDB Atlas
- Ir para: https://www.mongodb.com/cloud/atlas
- Criar conta gratuita
- Criar cluster

#### Passo 2: Obter Connection String
- No Atlas, ir para "Connect"
- Copiar connection string (ex: `mongodb+srv://user:pass@cluster.mongodb.net/caiman?retryWrites=true&w=majority`)

#### Passo 3: Configurar Backend
```bash
cp .env.example .env

# Editar .env:
MONGODB_URI=mongodb+srv://seu_usuario:sua_senha@seu_cluster.mongodb.net/caiman?retryWrites=true&w=majority
```

#### Passo 4: Executar
```bash
npm install
npm start
```

---

## 📡 Endpoints API

### Autenticação

#### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}

Response:
{
  "success": true,
  "message": "Login bem-sucedido",
  "token": "eyJhbGciOiJIUzI1NiIsInR...",
  "expiresIn": 604800,
  "user": {
    "id": "507f1f77bcf86cd799439011",
    "username": "admin",
    "email": "admin@caiman.panel"
  }
}
```

#### Registrar
```
POST /api/auth/register
Content-Type: application/json

{
  "username": "newuser",
  "password": "senha123",
  "email": "user@example.com",
  "licenseKey": "trial"
}

Response:
{
  "success": true,
  "message": "Registro bem-sucedido",
  "user": {
    "id": "507f1f77bcf86cd799439011",
    "username": "newuser",
    "email": "user@example.com"
  }
}
```

#### Refresh Token
```
POST /api/auth/refresh
Content-Type: application/json

{
  "token": "eyJhbGciOiJIUzI1NiIsInR..."
}

Response:
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR...",
  "expiresIn": 604800
}
```

### Painel

Todos os endpoints de painel requerem token Bearer na header:
```
Authorization: Bearer [token]
```

#### Aplicar Configurações
```
POST /api/panel/settings/apply
Content-Type: application/json
Authorization: Bearer [token]

{
  "aimbotEnabled": true,
  "rapidFireEnabled": false,
  "wallhackEnabled": true,
  "espEnabled": true,
  "aimbotDelay": 150,
  "espDistance": 300
}

Response:
{
  "success": true,
  "message": "Configurações aplicadas com sucesso",
  "settings": {...},
  "appliedAt": "2026-09-02T10:30:00Z"
}
```

#### Reiniciar Painel
```
POST /api/panel/restart
Authorization: Bearer [token]

Response:
{
  "success": true,
  "message": "Painel reiniciado com sucesso",
  "restartedAt": "2026-09-02T10:30:00Z"
}
```

#### Status do Painel
```
GET /api/panel/status
Authorization: Bearer [token]

Response:
{
  "success": true,
  "status": "connected",
  "message": "Painel conectado",
  "uptime": 3600,
  "timestamp": "2026-09-02T10:30:00Z"
}
```

#### Estatísticas
```
GET /api/panel/stats
Authorization: Bearer [token]

Response:
{
  "success": true,
  "stats": {
    "cpu": "15.42%",
    "memory": {
      "total": 8192,
      "used": 4096,
      "free": 4096,
      "percentage": "50.00%"
    },
    "uptime": "60 minutos",
    "timestamp": "2026-09-02T10:30:00Z"
  }
}
```

### Health Check

#### Básico
```
GET /api/health

Response:
{
  "success": true,
  "status": "healthy",
  "message": "CAIMAN Backend is running",
  "timestamp": "2026-09-02T10:30:00Z",
  "uptime": 3600
}
```

#### Detalhado
```
GET /api/health/detailed

Response:
{
  "success": true,
  "status": "healthy",
  "message": "CAIMAN Backend is running",
  "server": {
    "nodeVersion": "v18.0.0",
    "platform": "linux",
    "arch": "x64",
    "uptime": 3600
  },
  "system": {...},
  "database": {...},
  "timestamp": "2026-09-02T10:30:00Z"
}
```

---

## 🔐 Credenciais de Teste

**Usuário Demo:**
- Username: `admin`
- Password: `admin123`

> ⚠️ Mudar em produção!

---

## 📝 Variáveis de Ambiente

Editar arquivo `.env`:

```bash
# Banco de dados
MONGODB_URI=mongodb://localhost:27017/caiman

# JWT Secret (MUDAR EM PRODUÇÃO!)
JWT_SECRET=caiman-secret-key-change-in-production

# Servidor
PORT=3000
NODE_ENV=development
```

---

## 🛠️ Desenvolvimento

### Scripts Disponíveis

```bash
# Iniciar em produção
npm start

# Iniciar em desenvolvimento (auto-reload com nodemon)
npm run dev

# Testar
npm test

# Linter
npm run lint
```

---

## 📦 Estrutura de Pastas

```
Caiman-Backend/
├── server.js              ← Aplicação principal
├── package.json           ← Dependências
├── .env.example           ← Variáveis de ambiente (modelo)
├── docker-compose.yml     ← Docker orchestration
├── Dockerfile             ← Imagem Docker
├── README.md              ← Este arquivo
│
├── models/
│   └── User.js            ← Schema de usuário
│
└── routes/
    ├── auth.js            ← Endpoints autenticação
    ├── panel.js           ← Endpoints painel
    └── health.js          ← Health check
```

---

## 🔗 Integração com App Mobile

### Configurar URL do Backend

Editar `Caiman-Mobile/Services/ApiService.cs`:

```csharp
private const string BaseUrl = "http://seu-servidor:3000"; // Mudar para seu servidor
```

### Conectar App ao Backend

1. Backend rodando em `http://localhost:3000` (ou seu servidor)
2. Celular/emulador conectado à mesma rede (ou usar ngrok)
3. App pronto para fazer login/registrar

---

## 🚀 Deploy em Produção

### Opção 1: Heroku

```bash
# Instalar Heroku CLI
# Fazer login
heroku login

# Criar app
heroku create seu-app-name

# Configurar variáveis
heroku config:set MONGODB_URI=mongodb+srv://...
heroku config:set JWT_SECRET=seu-secret-aleatorio

# Deploy
git push heroku main
```

### Opção 2: Railway

```bash
# Instalar Railway CLI
npm i -g @railway/cli

# Login e conectar
railway login
railway init

# Deploy automático no push
git push
```

### Opção 3: Render

1. Conectar GitHub account
2. Criar novo "Web Service"
3. Conectar repositório
4. Configurar variáveis de ambiente
5. Deploy automático

---

## 🐛 Troubleshooting

### Erro: "Cannot connect to MongoDB"
```bash
✅ Verificar:
1. MongoDB está rodando? (docker-compose ps)
2. MONGODB_URI está correto em .env?
3. Firewall permite conexão?
```

### Erro: "EADDRINUSE: address already in use :::3000"
```bash
✅ Mudar PORT em .env ou:
lsof -i :3000
kill -9 <PID>
```

### Erro: "JWT token expired"
```bash
✅ App deve fazer refresh de token automaticamente
Verificar ApiService.cs - RefreshTokenAsync()
```

### App não conecta ao backend
```bash
✅ Verificar:
1. Backend rodando? (npm start)
2. URL correta em ApiService.cs?
3. Mesma rede (local) ou ngrok (remoto)?
4. CORS ativado? (verificar server.js)
```

---

## 🧪 Teste de Endpoints

### Com curl

```bash
# Login
curl -X POST http://localhost:3000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Copiar token da resposta e usar em:

# Status (substitua TOKEN)
curl -X GET http://localhost:3000/api/panel/status \
  -H "Authorization: Bearer TOKEN"
```

### Com Postman

1. Importar endpoints em Postman
2. Usar pasta "CAIMAN Backend" com requests pré-configuradas
3. Salvar em "Environments" para reusar tokens

---

## 📊 Monitoramento

### Logs em Tempo Real

```bash
# Com Docker
docker-compose logs -f backend

# Local
npm run dev
```

### Health Check

```bash
curl http://localhost:3000/api/health
```

Resposta OK = backend saudável

---

## 🔒 Segurança em Produção

**IMPORTANTE:**

1. ✅ Mudar `JWT_SECRET` em .env
2. ✅ Usar HTTPS (não HTTP)
3. ✅ Validar certificado SSL
4. ✅ Usar senha forte para MongoDB Atlas
5. ✅ Ativar 2FA em contas administrativas
6. ✅ Monitorar logs de erro
7. ✅ Rate limiting (implementar)
8. ✅ CORS apenas domínios autorizados

---

## 📈 Performance

- ✅ JWT tokens (sem database lookup)
- ✅ MongoDB indexes (username, email)
- ✅ HTTP connection pooling
- ✅ Compression middleware
- ✅ Caching de responses

---

## 📚 Documentação API Completa

Visitar após iniciar backend:
```
http://localhost:3000/api/docs
```
(Swagger UI - em desenvolvimento)

---

## 💬 Suporte

Erros ou dúvidas? Verificar:
1. Logs do backend (`npm run dev`)
2. Status MongoDB (`docker-compose ps`)
3. Health check (`curl http://localhost:3000/api/health`)

---

**Versão**: 1.0.0  
**Data**: 2026-09-02  
**Status**: 🟢 **PRONTO PARA PRODUÇÃO**

🐊 *CAIMAN Backend - Rápido, Seguro e Confiável* 🐊
