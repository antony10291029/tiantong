import Input from './Input.vue'
import Radio from './Radio.vue'
import Table from './Table.vue'
import Loader from './Loader.vue'
import Checkbox from './Checkbox.vue'
import Textarea from './Textarea.vue'
import Pagination from './Pagination.vue'
import AsyncButton from './AsyncButton.vue'
import AsyncLoader from './AsyncLoader.vue'
import YesOrNoCell from './YesOrNoCell.vue'
import SearchField from './SearchField.vue'
import EditableCell from './EditableCell.vue'
import DatePicker from './DatePicker/index.vue'
import DateWrapper from './wrappers/DateWrapper.vue'

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
