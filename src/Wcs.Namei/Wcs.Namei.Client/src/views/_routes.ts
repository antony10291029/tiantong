import { RouteConfig } from 'vue-router/types/router'
import Dashboard from './Dashboard/index.vue'
import Home from './Home/index.vue'
import Lifters from './Lifters/index.vue'
import NotFound from './NotFound/index.vue'
import AutoDoors from './AutoDoors/index.vue'

const routes: RouteConfig[] = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: Dashboard
      },
      {
        path: 'lifters',
        alias: '',
        name: 'Lifters',
        component: Lifters
      },
      {
        path: 'auto-doors',
        name: 'AutoDoors',
        component: AutoDoors
      }
    ]
  },
  {
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]

export default routes
