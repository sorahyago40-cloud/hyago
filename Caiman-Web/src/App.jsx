import { useState, useEffect } from 'react'
import './App.css'
import LoginPage from './pages/LoginPage'
import PanelPage from './pages/PanelPage'
import SettingsPage from './pages/SettingsPage'

export default function App() {
  const [currentPage, setCurrentPage] = useState('login')
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [user, setUser] = useState(null)
  const [token, setToken] = useState(null)

  useEffect(() => {
    const savedToken = localStorage.getItem('caiman_token')
    const savedUser = localStorage.getItem('caiman_user')

    if (savedToken && savedUser) {
      setToken(savedToken)
      setUser(JSON.parse(savedUser))
      setIsAuthenticated(true)
      setCurrentPage('panel')
    }
  }, [])

  const handleLogin = (userData, authToken) => {
    setUser(userData)
    setToken(authToken)
    setIsAuthenticated(true)
    setCurrentPage('panel')
    localStorage.setItem('caiman_token', authToken)
    localStorage.setItem('caiman_user', JSON.stringify(userData))
  }

  const handleLogout = () => {
    setUser(null)
    setToken(null)
    setIsAuthenticated(false)
    setCurrentPage('login')
    localStorage.removeItem('caiman_token')
    localStorage.removeItem('caiman_user')
  }

  return (
    <div className="app-container">
      <header className="app-header">
        <div className="header-content">
          <div className="logo">🐊 CAIMAN</div>
          {isAuthenticated && (
            <div className="user-info">
              <span>{user?.username}</span>
            </div>
          )}
        </div>
      </header>

      <main className="app-main">
        {!isAuthenticated ? (
          <LoginPage onLogin={handleLogin} />
        ) : (
          <>
            {currentPage === 'panel' && (
              <PanelPage token={token} user={user} />
            )}
            {currentPage === 'settings' && (
              <SettingsPage token={token} user={user} onLogout={handleLogout} />
            )}
          </>
        )}
      </main>

      {isAuthenticated && (
        <nav className="app-nav">
          <button
            className={`nav-btn ${currentPage === 'panel' ? 'active' : ''}`}
            onClick={() => setCurrentPage('panel')}
          >
            ⚙️ Painel
          </button>
          <button
            className={`nav-btn ${currentPage === 'settings' ? 'active' : ''}`}
            onClick={() => setCurrentPage('settings')}
          >
            ⚡ Config
          </button>
        </nav>
      )}
    </div>
  )
}
