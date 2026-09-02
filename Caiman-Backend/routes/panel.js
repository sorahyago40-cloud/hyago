const express = require('express');
const router = express.Router();

// Middleware to verify JWT token
const verifyToken = (req, res, next) => {
    const token = req.headers.authorization?.split(' ')[1];
    if (!token) {
        return res.status(401).json({ success: false, message: 'Token required' });
    }

    try {
        const jwt = require('jsonwebtoken');
        const decoded = jwt.verify(token, process.env.JWT_SECRET || 'caiman-secret-key-change-in-production');
        req.userId = decoded.userId;
        req.username = decoded.username;
        next();
    } catch (error) {
        return res.status(401).json({ success: false, message: 'Invalid token' });
    }
};

// Apply Settings
router.post('/settings/apply', verifyToken, async (req, res) => {
    try {
        const { aimbotEnabled, rapidFireEnabled, wallhackEnabled, espEnabled, aimbotDelay, espDistance } = req.body;

        // Validate settings
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

        // In a real application, save settings to database
        // For demo, just acknowledge receipt
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

// Restart Panel
router.post('/restart', verifyToken, async (req, res) => {
    try {
        // In a real application, trigger actual panel restart
        // For demo, just acknowledge
        res.json({
            success: true,
            message: 'Painel reiniciado com sucesso',
            restartedAt: new Date().toISOString()
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

// Get Panel Status
router.get('/status', verifyToken, async (req, res) => {
    try {
        res.json({
            success: true,
            status: 'connected',
            message: 'Painel conectado',
            uptime: process.uptime(),
            timestamp: new Date().toISOString()
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

// Get Panel Stats
router.get('/stats', verifyToken, async (req, res) => {
    try {
        const os = require('os');
        const totalMemory = os.totalmem();
        const freeMemory = os.freemem();
        const usedMemory = totalMemory - freeMemory;
        const memoryPercentage = ((usedMemory / totalMemory) * 100).toFixed(2);

        const cpus = os.cpus();
        const cpuUsage = (process.cpuUsage().user / 1000000).toFixed(2);

        res.json({
            success: true,
            stats: {
                cpu: `${cpuUsage}%`,
                memory: {
                    total: Math.round(totalMemory / 1024 / 1024),
                    used: Math.round(usedMemory / 1024 / 1024),
                    free: Math.round(freeMemory / 1024 / 1024),
                    percentage: `${memoryPercentage}%`
                },
                uptime: `${Math.floor(process.uptime() / 60)} minutos`,
                timestamp: new Date().toISOString()
            }
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

module.exports = router;
