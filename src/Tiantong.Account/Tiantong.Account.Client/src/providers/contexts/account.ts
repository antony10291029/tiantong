import Axios from './_axios'
import router from '../vue-router'
import { token } from '../local-storage'

const account = Axios.create(process.env.VUE_APP_ACCOUNT_URL);

export default {
  async sendVerificationEmail (address: string, subject: string, duration: number = 300) {
    const response = await account.post('/email-verifications/send', {
      address, subject, duration
    })

    return response.data.key
  },

  async registerByEmail (params: {
    email: string,
    name: string,
    password: string,
    verification_key: string,
    verification_code: string
  }) {
    const response = await account.post('/register/email', params)

    return response
  },

  async loginByEmail (params: {
    email: string,
    password: string
  }, redirect: string) {
    const response = await account.post('/login/email', params)
    const data = response.data
    const query = new URLSearchParams({
      token: data.token,
      expired_at: data.expired_at,
      refresh_at: data.refresh_at,
    })

    token.set(data)

    if (redirect === '') {
      router.push('/')
    } else {
      window.location.assign(`${atob(redirect)}?${query}`)
    }
    return response
  },

  async getProfile () {
    const response = await account.post('/person/data')

    return response.data
  },

  async verifyToken () {
    await account.post('/token/verify')
  },

  async updateProfile (name: string) {
    await account.post('./person/update', { name })
  },

  async resetPasswordByEmail (params: {
    email: string,
    password: string,
    verification_key: string,
    verification_code: string
  }) {
    await account.post('./password-reset/email', params)
  }
}
