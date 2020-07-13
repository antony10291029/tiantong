import domain from './context/domain'

export default {
  isLocked: false,

  isInitialized: false,

  async initialize () {
    let response = await domain.post('/system-lock/get')

    this.isLocked = response.data
    this.isInitialized = true
  },

  async unlock (password: string) {
    await domain.post('/system-lock/unlock', { password })
    this.isLocked = false
  }
}
