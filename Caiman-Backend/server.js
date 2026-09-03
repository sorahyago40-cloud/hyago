const express = require('express');
const cors = require('cors');
const dotenv = require('dotenv');
const mongoose = require('mongoose');

dotenv.config();

const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Database Connection (Optional for demo)
try {
    mongoose.connect(process.env.MONGODB_URI || 'mongodb://localhost:27017/caiman', {
        useNewUrlParser: true,
        useUnifiedTopology: true,
    })
    .then(() => console.log('✅ Database connected'))
    .catch(err => console.warn('⚠️  Database not available - using mock data'));
} catch (err) {
    console.warn('⚠️  MongoDB not available - using mock data');
}

// Routes
app.use('/api/auth', require('./routes/auth'));
app.use('/api/panel', require('./routes/panel'));
app.use('/api/health', require('./routes/health'));

// Error handling middleware
app.use((err, req, res, next) => {
    console.error(err);
    res.status(err.status || 500).json({
        success: false,
        message: err.message || 'Internal server error'
    });
});

// Start server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`🐊 CAIMAN Backend running on port ${PORT}`);
    console.log(`📍 http://localhost:${PORT}`);
    console.log(`🔗 API: http://localhost:${PORT}/api`);
});

module.exports = app;
