import { useState } from 'react'
import axios from 'axios'
import './LoginPage.css'

const API_URL = import.meta.env.VITE_API_URL || 'http://192.0.2.2:3000'

export default function LoginPage({ onLogin }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [email, setEmail] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')
  const [isRegister, setIsRegister] = useState(false)

  const handleLogin = async (e) => {
    e.preventDefault()
    setError('')
    setIsLoading(true)

    try {
      const response = await axios.post(`${API_URL}/api/auth/login`, {
        username,
        password
      })

      if (response.data.success) {
        onLogin(response.data.user, response.data.token)
      } else {
        setError(response.data.message || 'Login falhou')
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Erro ao conectar com servidor')
    } finally {
      setIsLoading(false)
    }
  }

  const handleRegister = async (e) => {
    e.preventDefault()
    setError('')
    setIsLoading(true)

    try {
      const response = await axios.post(`${API_URL}/api/auth/register`, {
        username,
        password,
        email
      })

      if (response.data.success) {
        setError('')
        setUsername('')
        setPassword('')
        setEmail('')
        setIsRegister(false)
        alert('Registrado com sucesso! Faça login agora.')
      } else {
        setError(response.data.message || 'Registro falhou')
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Erro ao registrar')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="login-container">
      <div className="login-box">
        <div className="login-header">
          <div className="login-emoji">🐊</div>
          <h1>CAIMAN</h1>
          <p>Panel de Controle</p>
        </div>

        <form onSubmit={isRegister ? handleRegister : handleLogin} className="login-form">
          <div className="form-group">
            <label>Usuário</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="admin"
              required
              disabled={isLoading}
            />
          </div>

          {isRegister && (
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="seu@email.com"
                required
                disabled={isLoading}
              />
            </div>
          )}

          <div className="form-group">
            <label>Senha</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
              required
              disabled={isLoading}
            />
          </div>

          {error && <div className="error-message">{error}</div>}

          <button
            type="submit"
            className="btn-primary btn-login"
            disabled={isLoading}
          >
            {isLoading ? (
              <>
                <span className="loading"></span> Aguarde...
              </>
            ) : (
              isRegister ? 'Registrar' : 'Entrar'
            )}
          </button>
        </form>

        <div className="login-footer">
          <button
            type="button"
            className="btn-link"
            onClick={() => {
              setIsRegister(!isRegister)
              setError('')
            }}
            disabled={isLoading}
          >
            {isRegister ? 'Já tem conta? Faça login' : 'Criar nova conta'}
          </button>
        </div>

        <div className="login-info">
          <p>🧪 Teste: admin / admin123</p>
        </div>
      </div>
    </div>
  )
}
