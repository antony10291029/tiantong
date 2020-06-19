import 'vue-class-component/hooks'
import Vue from 'vue'
import App from './views/App.vue'
import store from './store'
import uid from './providers/uid'
import notify from './providers/notify'
import router from './providers/router'
import confirm from './providers/confirm'
import directives from './providers/directives'
import components from './components'

Vue.config.productionTip = false;

Vue.use(uid)
Vue.use(directives)
Vue.use(notify)
Vue.use(confirm)
Vue.use(components)

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app')
