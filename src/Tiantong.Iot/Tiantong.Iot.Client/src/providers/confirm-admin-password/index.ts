import Component from './Component.vue'

export default {
  install (Vue: any) {
    const vm = new (Vue.extend(Component))().$mount()
    document.body.appendChild(vm.$el)
    Vue.prototype.$confirmAdminPassword = vm.open
  }
}
