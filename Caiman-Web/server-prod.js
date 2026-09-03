import express from 'express';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const app = express();

// Servir arquivos estáticos
app.use(express.static(path.join(__dirname, 'dist')));

// Servir index.html para todas as rotas (SPA)
app.use((req, res) => {
  res.sendFile(path.join(__dirname, 'dist', 'index.html'));
});

const PORT = process.env.PORT || 8080;
app.listen(PORT, '0.0.0.0', () => {
  console.log(`✅ Web App rodando em:`);
  console.log(`   http://localhost:${PORT}`);
  console.log(`   http://caiman.painel:${PORT}`);
  console.log(`   http://192.0.2.2:${PORT}`);
});
