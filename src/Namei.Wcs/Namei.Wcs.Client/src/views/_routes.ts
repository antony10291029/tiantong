import { RouteConfig } from 'vue-router/types/router'
import NotFound from './NotFound/index.vue'
import Dashboard from './Dashboard/index.vue'
import Home from './Home/index.vue'
import Lifters from './Lifters/index.vue'
import LifterStates from './Lifters.States/index.vue'
import LifterCommands from './Lifters.Commands/index.vue'
import LifterCommandsDashboard from './Lifters.Commands.Dashboard/index.vue'
import Doors from './Doors/index.vue'
import DoorsStates from './Doors.States/index.vue'
import DoorCommands from './DoorCommands/index.vue'
import DoorCommandsDashboard from './DoorCommands.Dashboard/index.vue'
import System from './System/index.vue'

const routes: RouteConfig[] = [
  {
    path: '/',
    name: 'Home',
    redirect: '/lifters',
    component: Home,
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: Dashboard
      },
      {
        path: 'lifters',
        name: 'Lifters',
        redirect: '/lifters/states',
        component: Lifters,
        children: [
          {
            path: 'states',
            name: 'LifterStates',
            component: LifterStates
          },
          {
            path: 'commands',
            name: 'LifterCommands',
            redirect: '/lifters/commands/1',
            component: LifterCommands,
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
        path: 'doors',
        name: 'Doors',
        redirect: '/doors/states',
        component: Doors,
        children: [
          {
            path: 'states',
            name: 'DoorStates',
            component: DoorsStates
          },
          {
            path: 'commands',
            name: 'DoorCommands',
            redirect: '/doors/commands/101',
            component: DoorCommands,
            children: [
              {
                path: ':doorId',
                name: 'DoorCommandsDashboard',
                component: DoorCommandsDashboard,
                props: route => ({ doorId: route.params.doorId }),
              }
            ]
          }
        ]
      },
      {
        path: 'system',
        name: 'System',
        component: System
      },
    ]
  },
  {
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]

export default routes
