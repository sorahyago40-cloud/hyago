import express from 'express';
import path from 'path';
import fs from 'fs';
import bcrypt from 'bcryptjs';
import jwt from 'jsonwebtoken';
import os from 'os';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const app = express();
const PORT = process.env.PORT || 3000;
const distPath = path.join(__dirname, 'dist');
const JWT_SECRET = process.env.JWT_SECRET || 'caiman-secret-key-change-in-production';
const DATA_DIR = path.join(__dirname, 'data');

// Ensure data directory exists
if (!fs.existsSync(DATA_DIR)) {
  fs.mkdirSync(DATA_DIR, { recursive: true });
}

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Mock users data
const mockUsersFile = path.join(DATA_DIR, 'users.json');
const getMockUsers = () => {
  if (!fs.existsSync(mockUsersFile)) {
    const defaultUsers = {
      admin: {
        username: 'admin',
        password: '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcg7b3XeKeUxWdeS86E36P4/KFm',
        email: 'admin@caiman.panel',
        status: 'active'
      }
    };
    fs.writeFileSync(mockUsersFile, JSON.stringify(defaultUsers, null, 2));
    return defaultUsers;
  }
  return JSON.parse(fs.readFileSync(mockUsersFile, 'utf-8'));
};

// JWT verification middleware
const verifyToken = (req, res, next) => {
  const token = req.headers.authorization?.split(' ')[1];
  if (!token) {
    return res.status(401).json({ success: false, message: 'Token required' });
  }
  try {
    const decoded = jwt.verify(token, JWT_SECRET);
    req.userId = decoded.userId;
    req.username = decoded.username;
    next();
  } catch (error) {
    return res.status(401).json({ success: false, message: 'Invalid token' });
  }
};

// Authentication Routes
app.post('/api/auth/login', async (req, res) => {
  try {
    const { username, password } = req.body;

    if (!username || !password) {
      return res.status(400).json({ success: false, message: 'Username and password required' });
    }

    const users = getMockUsers();
    const user = users[username];

    if (!user) {
      return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
    }

    const validPassword = await bcrypt.compare(password, user.password);
    if (!validPassword) {
      return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
    }

    const token = jwt.sign(
      { userId: username, username: username },
      JWT_SECRET,
      { expiresIn: '7d' }
    );

    res.json({
      success: true,
      message: 'Login realizado com sucesso',
      token: token,
      user: { username: user.username, email: user.email }
    });
  } catch (error) {
    console.error('Login error:', error);
    res.status(500).json({ success: false, message: 'Erro no servidor' });
  }
});

// Panel Routes
app.get('/api/panel/status', verifyToken, (req, res) => {
  res.json({
    success: true,
    status: 'connected',
    message: 'Painel conectado',
    uptime: process.uptime(),
    timestamp: new Date().toISOString()
  });
});

app.post('/api/panel/settings/apply', verifyToken, (req, res) => {
  try {
    const { aimbotEnabled, rapidFireEnabled, wallhackEnabled, espEnabled, aimbotDelay, espDistance } = req.body;

    if (typeof aimbotEnabled !== 'boolean' || typeof rapidFireEnabled !== 'boolean' ||
        typeof wallhackEnabled !== 'boolean' || typeof espEnabled !== 'boolean') {
      return res.status(400).json({ success: false, message: 'Invalid settings format' });
    }

    if (aimbotDelay < 50 || aimbotDelay > 500) {
      return res.status(400).json({ success: false, message: 'Aimbot delay must be between 50-500ms' });
    }

    if (espDistance < 10 || espDistance > 500) {
      return res.status(400).json({ success: false, message: 'ESP distance must be between 10-500m' });
    }

    res.json({
      success: true,
      message: 'Configurações aplicadas com sucesso',
      settings: {
        aimbotEnabled,
        rapidFireEnabled,
        wallhackEnabled,
        espEnabled,
        aimbotDelay,
        espDistance
      },
      appliedAt: new Date().toISOString()
    });
  } catch (error) {
    res.status(500).json({ success: false, message: error.message });
  }
});

app.post('/api/panel/restart', verifyToken, (req, res) => {
  res.json({
    success: true,
    message: 'Painel reiniciado com sucesso',
    restartedAt: new Date().toISOString()
  });
});

app.get('/api/panel/stats', verifyToken, (req, res) => {
  const totalMemory = os.totalmem();
  const freeMemory = os.freemem();
  const usedMemory = totalMemory - freeMemory;
  const memoryPercentage = ((usedMemory / totalMemory) * 100).toFixed(2);

  res.json({
    success: true,
    stats: {
      cpu: (process.cpuUsage().user / 1000000).toFixed(2) + '%',
      memory: {
        total: Math.round(totalMemory / 1024 / 1024),
        used: Math.round(usedMemory / 1024 / 1024),
        free: Math.round(freeMemory / 1024 / 1024),
        percentage: memoryPercentage + '%'
      },
      uptime: Math.floor(process.uptime() / 60) + ' minutos',
      timestamp: new Date().toISOString()
    }
  });
});

// Health check
app.get('/api/health', (req, res) => {
  res.json({ success: true, status: 'ok' });
});

// Serve static files
app.use(express.static(distPath, {
  maxAge: '1y',
  etag: false
}));

// SPA fallback - serve index.html for all other routes
app.use((req, res) => {
  res.setHeader('Content-Type', 'text/html; charset=utf-8');
  res.sendFile(path.join(distPath, 'index.html'));
});

app.listen(PORT, () => {
  console.log(`✅ Server running on port ${PORT}`);
});
