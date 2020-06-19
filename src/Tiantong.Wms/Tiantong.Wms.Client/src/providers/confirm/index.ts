import Dialog from './Dialog.vue'
import { ConfirmOptions } from '@/types/confirm'

let vm: any

export default {
  install (Vue: any) {
    Vue.prototype.$confirm = confirm
    vm = new (Vue.extend(Dialog))()
      .$mount('#confirmation')
  }
}

export function confirm (options : ConfirmOptions) {
  vm.open(options)
}
