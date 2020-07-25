import Dialog from './Dialog.vue'

export default {
  install (Vue: any) {
    const vm = new (Vue.extend(Dialog))().$mount()
    document.body.appendChild(vm.$el)
    Vue.prototype.$confirm = vm.open
  }
}
