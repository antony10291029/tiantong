import Input from './components/Input.vue'
import Radio from './components/Radio.vue'
import Table from './components/Table.vue'
import Loader from './components/Loader.vue'
import Switcher from './components/Switcher.vue'
import Checkbox from './components/Checkbox.vue'
import Textarea from './components/Textarea.vue'
import Pagination from './components/Pagination.vue'
import AsyncButton from './components/AsyncButton.vue'
import AsyncLoader from './components/AsyncLoader.vue'
import YesOrNoCell from './components/YesOrNoCell.vue'
import SearchField from './components/SearchField.vue'
import EditableCell from './components/EditableCell.vue'
import DatePicker from './components/DatePicker/index.vue'
import DateWrapper from './components/wrappers/DateWrapper.vue'
import Notify from './components/Notify'
import Confirm from './components/Confirm'
import directives from './directives'
import uid from './plugins/uid'

export default {
  install (Vue: any) {
    Vue.use(uid)
    Vue.use(directives)
    Vue.component('Input', Input)
    Vue.component('Radio', Radio)
    Vue.component('Table', Table)
    Vue.component('Loader', Loader)
    Vue.component('Switcher', Switcher)
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
    Vue.use(Confirm)
    Vue.use(Notify)
  }
}
