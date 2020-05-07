import Vue from 'vue'
import App from './views/App.vue'
import router from './providers/vue-router'
import store from './providers/vuex'
import components from './components/index'

Vue.use(components)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app')
