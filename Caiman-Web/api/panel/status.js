import { verifyToken, setCors } from '../_lib/auth.js';

export default function handler(req, res) {
  setCors(res);
  if (req.method === 'OPTIONS') return res.status(200).end();

  const decoded = verifyToken(req);
  if (!decoded) {
    return res.status(401).json({ success: false, message: 'Token required' });
  }

  res.status(200).json({
    success: true,
    status: 'connected',
    message: 'Painel conectado',
    timestamp: new Date().toISOString()
  });
}
