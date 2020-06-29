import NotFound from './public/NotFound.vue'
import { RouteConfig } from 'vue-router/types/router'

const routes: RouteConfig[] = [
  {
    path: '/',
    name: 'Home',
    redirect: '/profile',
    component: () => import('./Home.vue'),
    children: [
      {
        path: 'profile',
        name: 'Profile',
        component: () => import('./Profile/index.vue')
      }
    ]
  },
  {
    path: '/login',
    name: 'Login',
    props: route => ({
      redirect: route.query.redirect,
    }),
    component: () => import('./public/Login.vue'),
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('./public/Register.vue')
  },
  {
    path: '/reset-password/email',
    name: 'ResetPasswordByEmail',
    component: () => import('./public/ResetPasswordByEmail.vue')
  },
  {
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]

export default routes
