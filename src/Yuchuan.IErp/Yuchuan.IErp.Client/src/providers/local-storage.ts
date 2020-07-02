import store from 'store'

export const chanjet = {
  token: {
    get: () => store.get('chanjet.token', ''),
  
    set: (token: string) => store.set('chanjet.token', token),
  
    isExpired: () => !store.get('chanjet.token')
  },
  orgId: {
    get: () => store.get('chanjet.orgId', ''),

    set: (id: number) => store.set('chanjet.orgId', id),
  },
  orgCode: {
    get: () => store.get('chanjet.orgCode', ''),
    
    set: (code: string) => store.set('chanjet.orgCode', code)
  },
  bookCode: {
    get: () => store.get('chanjet.bookCode', ''),

    set: (code: string) => store.set('chanjet.bookCode', code)
  },
  clear () {
    store.remove('chanjet.token')
    store.remove('chanjet.orgId')
    store.remove('chanjet.orgCode')
    store.remove('chanjet.bookCode')
  }
}

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

export default {
  chanjet,
}
