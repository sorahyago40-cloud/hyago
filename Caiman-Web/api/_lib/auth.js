import jwt from 'jsonwebtoken';
import bcrypt from 'bcryptjs';

const JWT_SECRET = process.env.JWT_SECRET || 'caiman-secret-key-change-in-production';

const ADMIN_USER = {
  username: 'admin',
  passwordHash: '$2a$10$OvUe9I8/f7jn2kQzOI63OOVr5WC1cq4G.Z1meay/h5/I.qPkyClrG',
  email: 'admin@caiman.panel'
};

export function findUser(username) {
  if (username === 'admin') return ADMIN_USER;
  return null;
}

export async function checkPassword(plain, hash) {
  return bcrypt.compare(plain, hash);
}

export function signToken(username) {
  return jwt.sign({ userId: username, username }, JWT_SECRET, { expiresIn: '7d' });
}

export function setCors(res) {
  res.setHeader('Access-Control-Allow-Origin', '*');
  res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
  res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');
}

export function verifyToken(req) {
  const token = req.headers.authorization?.split(' ')[1];
  if (!token) return null;
  try {
    return jwt.verify(token, JWT_SECRET);
  } catch {
    return null;
  }
}
