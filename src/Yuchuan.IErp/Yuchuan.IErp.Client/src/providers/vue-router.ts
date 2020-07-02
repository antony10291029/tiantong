import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from '@/views/_routes'
import { account } from '@/providers/local-storage'

Vue.use(VueRouter)

const router = new VueRouter({
  routes,
  linkActiveClass: 'is-active'
})

router.beforeEach((to, from, next) => {
  if (to.meta.login === true && account.token.isExpired()) {
    const path = btoa(to.fullPath)
    const redirect = btoa(`${window.location.origin}/#/login/${path}`)

    window.location.assign(`https://account.als-yuchuan.com/#/login?redirect=${redirect}`)
  } else {
    next()
  }
})

export default router
