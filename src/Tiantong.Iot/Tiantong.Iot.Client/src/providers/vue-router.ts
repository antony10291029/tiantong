import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from '@/views/_routes'
import systemLock from './system-lock'

Vue.use(VueRouter)

const router = new VueRouter({
  routes,
  linkActiveClass: 'is-active'
})

router.beforeEach(async (to, from, next) => {
  if (to.path === '/unlock-system') {
    next()
  }

  if (!systemLock.isInitialized) {
    await systemLock.initialize()
  }

  if (systemLock.isLocked) {
    next('/unlock-system')
  } else {
    next()
  }
})

export default router
