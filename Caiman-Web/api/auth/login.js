import { findUser, checkPassword, signToken, setCors } from '../_lib/auth.js';

export default async function handler(req, res) {
  setCors(res);
  if (req.method === 'OPTIONS') return res.status(200).end();
  if (req.method !== 'POST') {
    return res.status(405).json({ success: false, message: 'Method not allowed' });
  }

  const { username, password } = req.body || {};
  if (!username || !password) {
    return res.status(400).json({ success: false, message: 'Usuário e senha obrigatórios' });
  }

  const user = findUser(username);
  if (!user) {
    return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
  }

  const valid = await checkPassword(password, user.passwordHash);
  if (!valid) {
    return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
  }

  const token = signToken(user.username);
  res.status(200).json({
    success: true,
    message: 'Login realizado com sucesso',
    token,
    user: { username: user.username, email: user.email }
  });
}
