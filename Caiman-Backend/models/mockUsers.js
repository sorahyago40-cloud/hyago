const fs = require('fs');
const path = require('path');
const bcrypt = require('bcryptjs');

const DATA_FILE = path.join(process.cwd(), 'data', 'users.json');

const ensureDir = () => {
  const dir = path.dirname(DATA_FILE);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
};

const loadUsers = () => {
  ensureDir();
  if (fs.existsSync(DATA_FILE)) {
    return JSON.parse(fs.readFileSync(DATA_FILE, 'utf8'));
  }
  return {};
};

const saveUsers = (users) => {
  ensureDir();
  fs.writeFileSync(DATA_FILE, JSON.stringify(users, null, 2));
};

const findUserByUsername = (username) => {
  const users = loadUsers();
  return users[username] || null;
};

const createUser = async (userData) => {
  const users = loadUsers();
  if (users[userData.username]) {
    throw new Error('User already exists');
  }

  const hashedPassword = await bcrypt.hash(userData.password, 10);
  const user = {
    ...userData,
    password: hashedPassword,
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    lastLogin: null
  };

  users[userData.username] = user;
  saveUsers(users);
  return user;
};

const updateUserLastLogin = (username) => {
  const users = loadUsers();
  if (users[username]) {
    users[username].lastLogin = new Date().toISOString();
    users[username].updatedAt = new Date().toISOString();
    saveUsers(users);
  }
};

const initializeMockData = async () => {
  const users = loadUsers();
  if (!users.admin) {
    await createUser({
      username: 'admin',
      password: 'admin123',
      email: 'admin@caiman.panel',
      status: 'active',
      licenseKey: 'default'
    });
  }
};

module.exports = {
  findUserByUsername,
  createUser,
  updateUserLastLogin,
  initializeMockData
};
