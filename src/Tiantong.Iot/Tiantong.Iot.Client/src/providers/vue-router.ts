import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from '@/views/_routes'
import systemLock from './system-lock'
import { account } from './local-storage'
import { login } from './context/account'

Vue.use(VueRouter)

const router = new VueRouter({
  routes,
  linkActiveClass: 'is-active'
})

router.beforeEach((to, from, next) => {
  if (to.meta.login === true && account.token.isExpired()) {
    login(to.fullPath)
  } else {
    next()
  }
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
