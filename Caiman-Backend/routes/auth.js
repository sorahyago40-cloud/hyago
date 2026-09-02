const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const { body, validationResult } = require('express-validator');
const User = require('../models/User');

const JWT_SECRET = process.env.JWT_SECRET || 'caiman-secret-key-change-in-production';

// Login
router.post('/login', [
    body('username').notEmpty().withMessage('Username required'),
    body('password').notEmpty().withMessage('Password required')
], async (req, res) => {
    try {
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return res.status(400).json({ success: false, message: 'Validation failed', errors: errors.array() });
        }

        const { username, password } = req.body;

        // Find user
        let user = await User.findOne({ username });
        if (!user) {
            // Create demo user for testing
            if (username === 'admin' && password === 'admin123') {
                user = new User({
                    username: 'admin',
                    password: await bcrypt.hash('admin123', 10),
                    email: 'admin@caiman.panel',
                    status: 'active'
                });
                await user.save();
            } else {
                return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
            }
        }

        // Check password
        const validPassword = await bcrypt.compare(password, user.password);
        if (!validPassword) {
            return res.status(401).json({ success: false, message: 'Credenciais inválidas' });
        }

        // Generate token
        const token = jwt.sign(
            { userId: user._id, username: user.username },
            JWT_SECRET,
            { expiresIn: '7d' }
        );

        // Update last login
        user.lastLogin = new Date();
        await user.save();

        res.json({
            success: true,
            message: 'Login bem-sucedido',
            token,
            expiresIn: 604800, // 7 dias em segundos
            user: {
                id: user._id,
                username: user.username,
                email: user.email
            }
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

// Register
router.post('/register', [
    body('username').notEmpty().isLength({ min: 3 }).withMessage('Username must be at least 3 characters'),
    body('password').notEmpty().isLength({ min: 6 }).withMessage('Password must be at least 6 characters'),
    body('email').isEmail().withMessage('Valid email required')
], async (req, res) => {
    try {
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return res.status(400).json({ success: false, message: 'Validation failed', errors: errors.array() });
        }

        const { username, password, email, licenseKey } = req.body;

        // Check if user exists
        let user = await User.findOne({ username });
        if (user) {
            return res.status(400).json({ success: false, message: 'Usuário já existe' });
        }

        // Hash password
        const hashedPassword = await bcrypt.hash(password, 10);

        // Create user
        user = new User({
            username,
            password: hashedPassword,
            email,
            licenseKey: licenseKey || 'trial',
            status: 'active'
        });

        await user.save();

        res.status(201).json({
            success: true,
            message: 'Registro bem-sucedido',
            user: {
                id: user._id,
                username: user.username,
                email: user.email
            }
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

// Refresh Token
router.post('/refresh', (req, res) => {
    try {
        const { token } = req.body;
        if (!token) {
            return res.status(400).json({ success: false, message: 'Token required' });
        }

        jwt.verify(token, JWT_SECRET, (err, decoded) => {
            if (err) {
                return res.status(401).json({ success: false, message: 'Invalid token' });
            }

            const newToken = jwt.sign(
                { userId: decoded.userId, username: decoded.username },
                JWT_SECRET,
                { expiresIn: '7d' }
            );

            res.json({
                success: true,
                token: newToken,
                expiresIn: 604800
            });
        });
    } catch (error) {
        res.status(500).json({ success: false, message: error.message });
    }
});

// Logout
router.post('/logout', (req, res) => {
    res.json({ success: true, message: 'Logout bem-sucedido' });
});

module.exports = router;
