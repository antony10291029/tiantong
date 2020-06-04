import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from '@/views/_routes'

Vue.use(VueRouter)

const router = new VueRouter({
  routes,
  linkActiveClass: 'is-active'
})

router.beforeEach(async (to, from, next) => {
  next()
})

export default router
