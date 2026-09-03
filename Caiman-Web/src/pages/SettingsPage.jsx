import { useState, useEffect } from 'react'
import axios from 'axios'
import './SettingsPage.css'

const API_URL = ''

export default function SettingsPage({ token, user, onLogout }) {
  const [stats, setStats] = useState(null)
  const [isLoading, setIsLoading] = useState(false)

  useEffect(() => {
    fetchStats()
    const interval = setInterval(fetchStats, 2000)
    return () => clearInterval(interval)
  }, [])

  const fetchStats = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/panel/stats`, {
        headers: { Authorization: `Bearer ${token}` }
      })
      if (response.data.success) {
        setStats(response.data.stats)
      }
    } catch (err) {
      // Silent error
    }
  }

  const handleLogout = () => {
    if (confirm('Tem certeza que deseja sair?')) {
      onLogout()
    }
  }

  const handleClearCache = () => {
    if (confirm('Limpar cache local?')) {
      localStorage.clear()
      if ('caches' in window) {
        caches.keys().then(names => {
          names.forEach(name => caches.delete(name))
        })
      }
      alert('Cache limpo!')
    }
  }

  return (
    <div className="settings-page">
      <div className="settings-section">
        <h2 className="section-title">👤 Conta</h2>
        <div className="setting-item">
          <label>Usuário</label>
          <p className="setting-value">{user?.username}</p>
        </div>
        <div className="setting-item">
          <label>Email</label>
          <p className="setting-value">{user?.email || 'Não informado'}</p>
        </div>
      </div>

      <div className="settings-section">
        <h2 className="section-title">⚡ Performance</h2>
        {stats ? (
          <>
            <div className="stat-item">
              <label>CPU</label>
              <div className="stat-bar">
                <div
                  className="stat-fill cpu"
                  style={{ width: '60%' }}
                ></div>
              </div>
              <span className="stat-value">{stats.cpu || '0%'}</span>
            </div>

            <div className="stat-item">
              <label>Memória</label>
              <div className="stat-bar">
                <div
                  className="stat-fill memory"
                  style={{
                    width: stats.memory?.percentage || '0%'
                  }}
                ></div>
              </div>
              <span className="stat-value">
                {stats.memory?.used || 0}MB / {stats.memory?.total || 0}MB
              </span>
            </div>

            <div className="stat-item">
              <label>Uptime</label>
              <p className="setting-value">{stats.uptime || 'N/A'}</p>
            </div>
          </>
        ) : (
          <p className="stat-loading">Carregando...</p>
        )}
      </div>

      <div className="settings-section">
        <h2 className="section-title">🛠️ Ferramentas</h2>
        <button
          className="btn-setting"
          onClick={handleClearCache}
        >
          🗑️ Limpar Cache
        </button>

        <button
          className="btn-setting"
          onClick={() => alert('Versão 1.0.0\n\nCaiman Panel Web PWA')}
        >
          ℹ️ Sobre
        </button>
      </div>

      <div className="settings-section">
        <h2 className="section-title">📱 App</h2>
        <div className="setting-item">
          <label>Adicionar à Tela Inicial</label>
          <p className="setting-hint">
            iOS: Toque em Compartilhar → Adicionar à Tela Inicial<br/>
            Android: Menu (⋯) → Instalar App
          </p>
        </div>
      </div>

      <div className="settings-actions">
        <button
          className="btn-logout"
          onClick={handleLogout}
          disabled={isLoading}
        >
          🚪 Sair da Conta
        </button>
      </div>

      <div className="settings-footer">
        <p>🐊 CAIMAN Panel v1.0.0</p>
        <p>Web PWA • Grátis • Sem Necessidade de Instalação</p>
      </div>
    </div>
  )
}
