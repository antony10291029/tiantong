import Home from './Home/index.vue'
import NotFound from './_public/NotFound.vue'

export default [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]
