import { verifyToken, setCors } from '../_lib/auth.js';

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

  res.status(200).json({
    success: true,
    message: 'Painel reiniciado com sucesso',
    restartedAt: new Date().toISOString()
  });
}
