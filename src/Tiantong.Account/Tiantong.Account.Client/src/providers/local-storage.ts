import store from 'store'

interface tokenContext {
  token: string,
  expired_at: string,
  refresh_at: string
}

export const token = {
  cache: null as any,

  isUpdated: false,

  set (token: tokenContext) {
    this.cache = token
    store.set('token', JSON.stringify(token))
  },

  get (): tokenContext {
    if (!this.cache) {
      this.cache = JSON.parse(store.get('token') || null)
    }

    return this.cache
  },

  clear () {
    this.cache = null
    store.remove('token')
  }
}

export default {
  token
}
