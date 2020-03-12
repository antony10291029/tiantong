import store from 'store'

export default {
  token: {
    get () : string {
      return store.get('token')
    },
    set (token : string) : void {
      store.set('token', token)
    },
    clear () : void {
      store.remove('token')
    }
  }
}
