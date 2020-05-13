import Input from './global/Input.vue'
import Radio from './global/Radio.vue'
import Table from './global/Table.vue'
import Loader from './global/Loader.vue'
import Checkbox from './global/Checkbox.vue'
import Textarea from './global/Textarea.vue'
import Pagination from './global/Pagination.vue'
import AsyncButton from './global/AsyncButton.vue'
import AsyncLoader from './global/AsyncLoader.vue'
import YesOrNoCell from './global/YesOrNoCell.vue'
import SearchField from './global/SearchField.vue'
import EditableCell from './global/EditableCell.vue'
import DatePicker from './global/DatePicker/index.vue'
import DateWrapper from './global/wrappers/DateWrapper.vue'

export default {
  install (Vue: any) {
    Vue.component('Input', Input)
    Vue.component('Radio', Radio)
    Vue.component('Table', Table)
    Vue.component('Loader', Loader)
    Vue.component('Checkbox', Checkbox)
    Vue.component('Textarea', Textarea)
    Vue.component('DatePicker', DatePicker)
    Vue.component('Pagination', Pagination)
    Vue.component('AsyncButton', AsyncButton)
    Vue.component('AsyncLoader', AsyncLoader)
    Vue.component('DateWrapper', DateWrapper)
    Vue.component('SearchField', SearchField)
    Vue.component('YesOrNoCell', YesOrNoCell)
    Vue.component('EditableCell', EditableCell)
  }
}
