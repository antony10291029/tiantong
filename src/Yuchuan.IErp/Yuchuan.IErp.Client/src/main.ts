import 'vue-class-component/hooks'
import Vue from 'vue'
import App from './views/App.vue'
import router from './providers/vue-router'
import VueBulma from '@zhanglan1315/vue-bulma/src/share'

Vue.use(VueBulma)

Vue.config.productionTip = false

new Vue({
  router,
  render: (h) => h(App),
}).$mount('#app')
