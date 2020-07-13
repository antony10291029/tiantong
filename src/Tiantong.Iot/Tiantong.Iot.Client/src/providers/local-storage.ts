import store from 'store'

export const account = {
  token: {
    get: () => store.get('account.token', ''),

    set: (token: string) => store.set('account.token', token),

    isExpired: () => !store.get('account.token')
  },

  clear () {
    store.remove('account.token')
  }
}
