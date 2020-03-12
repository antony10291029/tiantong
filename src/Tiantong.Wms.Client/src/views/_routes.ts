const routes = [
  {
    path: '/',
    redirect: '/home'
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('./Home.vue'),
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('./Public/Register.vue'),
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('./Public/Login.vue'),
  },
  {
    path: '/unauthorization',
    name: 'Unthorization',
    component: () => import('./Public/Unauthorization.vue')
  }
]

export default routes
