import { setCors } from './_lib/auth.js';

export default function handler(req, res) {
  setCors(res);
  res.status(200).json({ success: true, status: 'ok' });
}
