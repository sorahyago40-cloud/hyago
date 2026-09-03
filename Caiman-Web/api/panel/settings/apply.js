import { verifyToken, setCors } from '../../_lib/auth.js';

export default function handler(req, res) {
  setCors(res);
  if (req.method === 'OPTIONS') return res.status(200).end();

  const decoded = verifyToken(req);
  if (!decoded) {
    return res.status(401).json({ success: false, message: 'Token required' });
  }

  if (req.method !== 'POST') {
    return res.status(405).json({ success: false, message: 'Method not allowed' });
  }

  const { aimbotEnabled, rapidFireEnabled, wallhackEnabled, espEnabled, aimbotDelay, espDistance } = req.body || {};

  if (
    typeof aimbotEnabled !== 'boolean' ||
    typeof rapidFireEnabled !== 'boolean' ||
    typeof wallhackEnabled !== 'boolean' ||
    typeof espEnabled !== 'boolean'
  ) {
    return res.status(400).json({ success: false, message: 'Invalid settings format' });
  }

  if (aimbotDelay < 50 || aimbotDelay > 500) {
    return res.status(400).json({ success: false, message: 'Aimbot delay must be between 50-500ms' });
  }

  if (espDistance < 10 || espDistance > 500) {
    return res.status(400).json({ success: false, message: 'ESP distance must be between 10-500m' });
  }

  res.status(200).json({
    success: true,
    message: 'Configurações aplicadas com sucesso',
    settings: { aimbotEnabled, rapidFireEnabled, wallhackEnabled, espEnabled, aimbotDelay, espDistance },
    appliedAt: new Date().toISOString()
  });
}
