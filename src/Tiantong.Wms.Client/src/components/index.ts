import Radio from './global/Radio.vue'
import Checkbox from './global/Checkbox.vue'

export default {
  install (Vue: any) {
    Vue.component('Radio', Radio)
    Vue.component('Checkbox', Checkbox)
  }
}