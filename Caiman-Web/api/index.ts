import { readFileSync } from 'fs';
import { join } from 'path';
import type { VercelRequest, VercelResponse } from '@vercel/node';

export default function handler(req: VercelRequest, res: VercelResponse) {
  try {
    const distPath = join(__dirname, '..', 'dist');
    let filePath = join(distPath, req.url === '/' ? 'index.html' : req.url);

    try {
      const stats = require('fs').statSync(filePath);
      if (stats.isDirectory()) {
        filePath = join(filePath, 'index.html');
      }
    } catch {
      filePath = join(distPath, 'index.html');
    }

    const content = readFileSync(filePath, 'utf-8');
    const ext = filePath.split('.').pop();

    const mimeTypes: Record<string, string> = {
      html: 'text/html',
      js: 'application/javascript',
      css: 'text/css',
      json: 'application/json',
      svg: 'image/svg+xml',
      png: 'image/png',
      jpg: 'image/jpeg',
      ico: 'image/x-icon'
    };

    res.setHeader('Content-Type', mimeTypes[ext!] || 'text/html');
    if (['js', 'css'].includes(ext!)) {
      res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
    }
    return res.status(200).send(content);
  } catch (e) {
    return res.status(500).send('Error');
  }
}
