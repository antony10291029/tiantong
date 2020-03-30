import Radio from './global/Radio.vue'
import Checkbox from './global/Checkbox.vue'
import EditableCell from './EditableCell.vue'

export default {
  install (Vue: any) {
    Vue.component('Radio', Radio)
    Vue.component('Checkbox', Checkbox)
    Vue.component('EditableCell', EditableCell)
  }
}
