import Vue from 'vue'
import App from './views/App.vue'
import uid from './providers/uid'
import store from './providers/vuex'
import notify from './providers/notify'
import confirm from './providers/confirm'
import router from './providers/vue-router'
import components from './components/index'
import directives from './providers/directives'

Vue.use(uid)
Vue.use(notify)
Vue.use(confirm)
Vue.use(directives)
Vue.use(components)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app')
