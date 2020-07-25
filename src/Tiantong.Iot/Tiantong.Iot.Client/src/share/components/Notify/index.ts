import Notification from './NotificationContainer.vue'

let vm: any

function add (text: string, type = 'success', duration = 3333) {
  vm.add(text, type, duration)
}

function info (text: string, duration = 3333) {
  vm.add(text, 'info', duration)
}

function link (text: string, duration = 3333) {
  vm.add(text, 'link', duration)
}

function danger (text: string, duration = 3333) {
  vm.add(text, 'danger', duration)
}

function success (text: string, duration = 3333) {
  vm.add(text, 'success', duration)
}

function warning (text: string, duration = 3333) {
  vm.add(text, 'warning', duration)
}

const notify = {
  add,
  info,
  link,
  danger,
  success,
  warning,
}

export default {
  install (Vue: any) {
    vm = new (Vue.extend(Notification))().$mount()
    document.body.appendChild(vm.$el)
    Vue.prototype.$notify = notify
  }
}
