import Vue from 'vue'
import App from './views/App.vue'
import router from './providers/vue-router'
import components from './share'
import confirmAdminPassword from './providers/confirm-admin-password'

Vue.use(components)
Vue.use(confirmAdminPassword)

Vue.config.productionTip = false

new Vue({
  router,
  render: (h) => h(App),
}).$mount('#app')
