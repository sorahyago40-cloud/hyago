# Deploy na Vercel

## Passo 1: Criar Conta Vercel
1. Acesse: https://vercel.com
2. Clique em "Sign Up"
3. Faça login com GitHub (usar sua conta sorahyago40-cloud)

## Passo 2: Conectar Repositório
1. Na dashboard da Vercel, clique em "New Project"
2. Selecione o repositório "sorahyago40-cloud/hyago"
3. Escolha a pasta: `Caiman-Web`

## Passo 3: Configurar Variáveis de Ambiente
Na seção "Environment Variables" adicione:
- **Nome**: `VITE_API_URL`
- **Valor**: `https://seu-backend.com` (ou deixe como está para desenvolvimento)

## Passo 4: Deploy
1. Clique em "Deploy"
2. Aguarde o build completar (2-3 minutos)
3. Você receberá um link tipo: `https://seu-projeto.vercel.app`

## URLs de Acesso
Após o deploy:
- **Frontend**: https://seu-projeto.vercel.app
- **Login**: admin / admin123

## Alternativa: Deploy Automático
Se configurar corretamente os secrets do GitHub:
- `VERCEL_TOKEN`
- `VERCEL_ORG_ID`  
- `VERCEL_PROJECT_ID`

O deploy será automático a cada push! ✅

---

**Nota**: O backend ainda rodará localmente. Para produção, você precisará fazer deploy do backend também em um serviço como Render.com, Railway, ou Heroku.
