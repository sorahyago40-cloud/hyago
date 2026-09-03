import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));

export default function handler(req, res) {
  try {
    const distPath = path.join(__dirname, '..', 'dist');
    let filePath = path.join(distPath, req.url === '/' ? 'index.html' : req.url.substring(1));

    if (!fs.existsSync(filePath) || fs.statSync(filePath).isDirectory()) {
      filePath = path.join(distPath, 'index.html');
    }

    const content = fs.readFileSync(filePath, 'utf-8');
    const ext = path.extname(filePath);

    const mimeTypes = {
      '.html': 'text/html',
      '.js': 'application/javascript',
      '.css': 'text/css',
      '.json': 'application/json',
      '.svg': 'image/svg+xml',
      '.png': 'image/png',
      '.jpg': 'image/jpeg',
      '.jpeg': 'image/jpeg',
      '.ico': 'image/x-icon'
    };

    res.setHeader('Content-Type', mimeTypes[ext] || 'application/octet-stream');
    if (['.js', '.css'].includes(ext)) {
      res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
    }
    return res.status(200).send(content);
  } catch (e) {
    console.error(e);
    return res.status(500).send('Error');
  }
}
