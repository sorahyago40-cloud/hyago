import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const distPath = path.join(__dirname, '..', 'dist');

export default function handler(req, res) {
  let filePath = path.join(distPath, req.query.path ? req.query.path.join('/') : 'index.html');

  // Se o arquivo não existe ou é uma rota SPA, serve index.html
  if (!fs.existsSync(filePath) || fs.statSync(filePath).isDirectory()) {
    filePath = path.join(distPath, 'index.html');
  }

  try {
    const content = fs.readFileSync(filePath, 'utf-8');
    const ext = path.extname(filePath);

    let contentType = 'text/html';
    if (ext === '.js') contentType = 'application/javascript';
    if (ext === '.css') contentType = 'text/css';
    if (ext === '.json') contentType = 'application/json';
    if (ext === '.svg') contentType = 'image/svg+xml';
    if (ext === '.png') contentType = 'image/png';
    if (ext === '.jpg' || ext === '.jpeg') contentType = 'image/jpeg';
    if (ext === '.ico') contentType = 'image/x-icon';
    if (ext === '.woff2') contentType = 'font/woff2';

    res.setHeader('Content-Type', contentType);
    if (ext === '.js' || ext === '.css') {
      res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
    }
    return res.status(200).send(content);
  } catch (err) {
    return res.status(500).send('Server Error');
  }
}
