import express from 'express';
import path from 'path';
import fs from 'fs';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const app = express();
const PORT = process.env.PORT || 3000;
const distPath = path.resolve(__dirname, 'dist');

// CORS
app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
  res.header('Access-Control-Allow-Headers', 'Content-Type, Authorization');
  if (req.method === 'OPTIONS') return res.sendStatus(200);
  next();
});

// Serve files manually with correct MIME types
app.get('*.js', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'application/javascript; charset=utf-8');
  res.sendFile(filePath);
});

app.get('*.css', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'text/css; charset=utf-8');
  res.sendFile(filePath);
});

app.get('*.json', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'application/json; charset=utf-8');
  res.sendFile(filePath);
});

app.get('*.svg', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'image/svg+xml');
  res.sendFile(filePath);
});

app.get('*.png', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'image/png');
  res.sendFile(filePath);
});

app.get('*.ico', (req, res) => {
  const filePath = path.join(distPath, req.path);
  res.setHeader('Content-Type', 'image/x-icon');
  res.sendFile(filePath);
});

// Fallback to index.html for SPA
app.use((req, res) => {
  const indexPath = path.join(distPath, 'index.html');
  res.setHeader('Content-Type', 'text/html; charset=utf-8');
  res.sendFile(indexPath);
});

app.listen(PORT, () => {
  console.log(`✅ Server on port ${PORT}`);
});
