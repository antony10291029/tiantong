import { ValidationProvider, ValidationObserver, extend, localize } from 'vee-validate'
import { required, email, min, confirmed } from 'vee-validate/dist/rules'
import VeeErrors from './vee-errors.vue'
import zh from './zh.json'

extend('min', min)
extend('email', email)
extend('required', required)
extend('confirmed', confirmed)

localize({
  zh
})

export default {
  install (Vue: any): any {
    localize('zh')
    Vue.component('vee-errors', VeeErrors)
    Vue.component('vee-provider', ValidationProvider)
    Vue.component('vee-observer', ValidationObserver)
  }
}
