import { RouteConfig } from 'vue-router/types/router'
import Dashboard from './Dashboard/index.vue'
import Home from './Home/index.vue'
import Lifters from './Lifters/index.vue'
import NotFound from './NotFound/index.vue'
import AutoDoors from './AutoDoors/index.vue'
import LiftersDebug from './LiftersDebug/index.vue'
import LiftersDebugView from './LiftersDebug.View/index.vue'
import System from './System/index.vue'

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
      },
      {
        path: 'system',
        name: 'System',
        component: System
      },
      {
        path: 'lifters-debug',
        name: 'LiftersDebug',
        component: LiftersDebug,
        children: [
          {
            path: ':lifterId',
            name: 'LiftersDebugView',
            component: LiftersDebugView,
            props: route => ({ lifterId: +route.params.lifterId })
          }
        ]
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
