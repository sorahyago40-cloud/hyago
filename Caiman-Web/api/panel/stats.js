import os from 'os';
import { verifyToken, setCors } from '../_lib/auth.js';

export default function handler(req, res) {
  setCors(res);
  if (req.method === 'OPTIONS') return res.status(200).end();

  const decoded = verifyToken(req);
  if (!decoded) {
    return res.status(401).json({ success: false, message: 'Token required' });
  }

  const totalMemory = os.totalmem();
  const freeMemory = os.freemem();
  const usedMemory = totalMemory - freeMemory;
  const memoryPercentage = ((usedMemory / totalMemory) * 100).toFixed(2);

  res.status(200).json({
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
}
