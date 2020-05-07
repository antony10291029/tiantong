import Vue from 'vue'
import App from './views/App.vue'
import router from './providers/vue-router'
import store from './providers/vuex'

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app')
