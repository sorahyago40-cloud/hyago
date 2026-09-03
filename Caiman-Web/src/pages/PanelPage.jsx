import { useState, useEffect } from 'react'
import axios from 'axios'
import './PanelPage.css'

const API_URL = ''

export default function PanelPage({ token, user }) {
  const [settings, setSettings] = useState({
    aimbotEnabled: false,
    rapidFireEnabled: false,
    wallhackEnabled: false,
    espEnabled: false,
    aimbotDelay: 150,
    espDistance: 300
  })

  const [status, setStatus] = useState('connecting')
  const [message, setMessage] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  useEffect(() => {
    checkStatus()
    const interval = setInterval(checkStatus, 5000)
    return () => clearInterval(interval)
  }, [])

  const checkStatus = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/panel/status`, {
        headers: { Authorization: `Bearer ${token}` }
      })
      setStatus('connected')
    } catch (err) {
      setStatus('disconnected')
    }
  }

  const handleToggle = (key) => {
    setSettings(prev => ({
      ...prev,
      [key]: !prev[key]
    }))
  }

  const handleSliderChange = (key, value) => {
    setSettings(prev => ({
      ...prev,
      [key]: parseInt(value)
    }))
  }

  const handleApplySettings = async () => {
    setIsLoading(true)
    setMessage('')

    try {
      const response = await axios.post(
        `${API_URL}/api/panel/settings/apply`,
        settings,
        { headers: { Authorization: `Bearer ${token}` } }
      )

      if (response.data.success) {
        setMessage('✅ Configurações aplicadas com sucesso!')
        setTimeout(() => setMessage(''), 3000)
      } else {
        setMessage('❌ Erro ao aplicar configurações')
      }
    } catch (err) {
      setMessage('❌ Erro na comunicação com servidor')
    } finally {
      setIsLoading(false)
    }
  }

  const handleRestart = async () => {
    if (!confirm('Tem certeza que deseja reiniciar o painel?')) return

    setIsLoading(true)
    setMessage('')

    try {
      const response = await axios.post(
        `${API_URL}/api/panel/restart`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      )

      if (response.data.success) {
        setMessage('✅ Painel reiniciado com sucesso!')
        setTimeout(() => setMessage(''), 3000)
      }
    } catch (err) {
      setMessage('❌ Erro ao reiniciar painel')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="panel-page">
      <div className="panel-status">
        <span className={`status-badge ${status}`}>
          {status === 'connected' ? '🟢 Online' : '🔴 Offline'}
        </span>
      </div>

      {message && (
        <div className={`message ${message.includes('✅') ? 'success' : 'error'}`}>
          {message}
        </div>
      )}

      <div className="panel-section">
        <h2 className="section-title">🎯 Aimbot</h2>
        <div className="toggle">
          <span className="toggle-label">Aimbot Ativado</span>
          <div
            className={`toggle-switch ${settings.aimbotEnabled ? 'active' : ''}`}
            onClick={() => handleToggle('aimbotEnabled')}
          ></div>
        </div>

        {settings.aimbotEnabled && (
          <div className="slider-container">
            <div className="slider-label">
              <span>Delay (ms)</span>
              <span className="slider-value">{settings.aimbotDelay}ms</span>
            </div>
            <input
              type="range"
              min="50"
              max="500"
              step="10"
              value={settings.aimbotDelay}
              onChange={(e) => handleSliderChange('aimbotDelay', e.target.value)}
            />
            <div className="slider-hint">50ms (rápido) ↔ 500ms (lento)</div>
          </div>
        )}
      </div>

      <div className="panel-section">
        <h2 className="section-title">💨 RapidFire</h2>
        <div className="toggle">
          <span className="toggle-label">RapidFire Ativado</span>
          <div
            className={`toggle-switch ${settings.rapidFireEnabled ? 'active' : ''}`}
            onClick={() => handleToggle('rapidFireEnabled')}
          ></div>
        </div>
      </div>

      <div className="panel-section">
        <h2 className="section-title">👁️ Wallhack</h2>
        <div className="toggle">
          <span className="toggle-label">Wallhack Ativado</span>
          <div
            className={`toggle-switch ${settings.wallhackEnabled ? 'active' : ''}`}
            onClick={() => handleToggle('wallhackEnabled')}
          ></div>
        </div>
      </div>

      <div className="panel-section">
        <h2 className="section-title">📍 ESP</h2>
        <div className="toggle">
          <span className="toggle-label">ESP Ativado</span>
          <div
            className={`toggle-switch ${settings.espEnabled ? 'active' : ''}`}
            onClick={() => handleToggle('espEnabled')}
          ></div>
        </div>

        {settings.espEnabled && (
          <div className="slider-container">
            <div className="slider-label">
              <span>Distância (m)</span>
              <span className="slider-value">{settings.espDistance}m</span>
            </div>
            <input
              type="range"
              min="10"
              max="500"
              step="10"
              value={settings.espDistance}
              onChange={(e) => handleSliderChange('espDistance', e.target.value)}
            />
            <div className="slider-hint">10m (perto) ↔ 500m (longe)</div>
          </div>
        )}
      </div>

      <div className="panel-actions">
        <button
          className="btn-primary"
          onClick={handleApplySettings}
          disabled={isLoading}
        >
          {isLoading ? '⏳ Aplicando...' : '✅ Aplicar Configurações'}
        </button>

        <button
          className="btn-secondary"
          onClick={handleRestart}
          disabled={isLoading}
        >
          🔄 Reiniciar Painel
        </button>
      </div>

      <div className="panel-controls">
        <h2 className="section-title">⚙️ Controles do Sistema</h2>

        <div className="control-group">
          <button className="btn-control btn-update">
            📦 Atualizar Sistema
          </button>
          <button className="btn-control btn-stats">
            📊 Ver Estatísticas
          </button>
        </div>

        <div className="control-group">
          <button className="btn-control btn-save">
            💾 Salvar Configurações
          </button>
          <button className="btn-control btn-reset">
            🔄 Resetar para Padrão
          </button>
        </div>

        <div className="control-group">
          <button className="btn-control btn-backup">
            🔐 Fazer Backup
          </button>
          <button className="btn-control btn-info">
            ℹ️ Informações do Sistema
          </button>
        </div>
      </div>

      <div className="panel-info">
        <p>💡 Dica: Ajuste os valores e clique em "Aplicar Configurações"</p>
        <p>👤 Usuário: <strong>{user?.username || user}</strong></p>
        <p>🔌 Status: <strong>{status === 'connected' ? 'Conectado' : 'Desconectado'}</strong></p>
      </div>
    </div>
  )
}
