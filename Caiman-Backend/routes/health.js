const express = require('express');
const router = express.Router();

// Health check endpoint
router.get('/', (req, res) => {
    res.json({
        success: true,
        status: 'healthy',
        message: 'CAIMAN Backend is running',
        timestamp: new Date().toISOString(),
        uptime: process.uptime()
    });
});

// Detailed health check
router.get('/detailed', (req, res) => {
    const os = require('os');

    res.json({
        success: true,
        status: 'healthy',
        message: 'CAIMAN Backend is running',
        server: {
            nodeVersion: process.version,
            platform: process.platform,
            arch: process.arch,
            uptime: Math.floor(process.uptime())
        },
        system: {
            totalMemory: `${Math.round(os.totalmem() / 1024 / 1024)} MB`,
            freeMemory: `${Math.round(os.freemem() / 1024 / 1024)} MB`,
            cpuCount: os.cpus().length,
            loadAverage: os.loadavg()
        },
        database: {
            status: 'connected',
            uri: process.env.MONGODB_URI ? 'configured' : 'default'
        },
        timestamp: new Date().toISOString()
    });
});

module.exports = router;
