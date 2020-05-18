import axios from './axios'

export default {
  isLocked: false,

  isInitialized: false,

  async initialize () {
    let response = await axios.post('/system-lock/get')

    this.isLocked = response.data
    this.isInitialized = true
  },

  async unlock (password: string) {
    await axios.post('/system-lock/unlock', { password })
    this.isLocked = false
  }
}
