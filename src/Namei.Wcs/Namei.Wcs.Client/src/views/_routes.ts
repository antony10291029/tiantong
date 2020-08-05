import { RouteConfig } from 'vue-router/types/router'
import Dashboard from './Dashboard/index.vue'
import Home from './Home/index.vue'
import Lifters from './Lifters/index.vue'
import NotFound from './NotFound/index.vue'
import AutoDoors from './AutoDoors/index.vue'
import LifterCommands from './LifterCommands/index.vue'
import LifterCommandsDashboard from './LifterCommands.Dashboard/index.vue'
import DoorCommands from './DoorCommands/index.vue'
import DoorCommandsDashboard from './DoorCommands.Dashboard/index.vue'
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
        path: 'door-commands',
        name: 'DoorCommands',
        redirect: '/door-commands/101',
        component: DoorCommands,
        children: [
          {
            path: ':doorId',
            name: 'DoorCommandsDashboard',
            component: DoorCommandsDashboard,
            props: route => ({ doorId: route.params.doorId }),
          }
        ]
      },
      {
        path: 'lifter-commands',
        name: 'LifterCommands',
        component: LifterCommands,
        redirect: '/lifter-commands/1',
        children: [
          {
            path: ':lifterId',
            name: 'LifterCommandsDashboard',
            component: LifterCommandsDashboard,
            props: route => ({ lifterId: route.params.lifterId })
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
