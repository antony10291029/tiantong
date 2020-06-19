import Vue from 'vue'
import axios from './axios'
import router from './router'
import localStorage from './local-storage'

const state = Vue.observable({
  token: '',
  isAuthed: false,
  isInitialized: false
})

export default {
  state,
  set (token: string) {
    state.token = token
    state.isAuthed = true
    localStorage.token.set(token)
  },
  get (): string {
    if (state.token === '') {
      state.token = localStorage.token.get()
    }

    return state.token
  },
  clear (): void {
    state.isAuthed = false
    localStorage.token.clear()
  },
  async refresh () {
    var response = await axios.post('auth/token/refresh')
    var token = response.data.token
    state.isAuthed = true
    this.set(token)
  },
  async initialize () {
    if (state.isInitialized) return
    if (this.get()) {
      await this.refresh()
    } else {
      router.push('/login')
    }

    state.isInitialized = true
  },
}
