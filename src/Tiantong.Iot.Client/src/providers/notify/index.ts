import Notification from './NotificationContainer.vue'

let vm: any

export function add (text: string, type = 'success', duration = 3333) {
  vm.add(text, type, duration)
}

export function info (text: string, duration = 3333) {
  vm.add(text, 'info', duration)
}

export function link (text: string, duration = 3333) {
  vm.add(text, 'link', duration)
}

export function danger (text: string, duration = 3333) {
  vm.add(text, 'danger', duration)
}

export function success (text: string, duration = 3333) {
  vm.add(text, 'success', duration)
}

export function warning (text: string, duration = 3333) {
  vm.add(text, 'warning', duration)
}

export const notify = {
  add,
  info,
  link,
  danger,
  success,
  warning,
}

export default {
  install (Vue: any) {
    Vue.prototype.$notify = notify
    vm = new (Vue.extend(Notification))()
      .$mount('#notifications')
  }
}
