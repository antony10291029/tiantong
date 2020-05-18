import Component from './Component.vue'

let vm: any

export default {
  install (Vue: any) {
    Vue.prototype.$confirmAdminPassword = confirmAdminPassword
    vm = new (Vue.extend(Component))()
      .$mount('#confirm-admin-password')
  }
}

export function confirmAdminPassword (handler: () => {}) {
  vm.open(handler)
}
