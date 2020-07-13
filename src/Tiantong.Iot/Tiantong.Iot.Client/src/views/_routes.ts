import { RouteConfig } from 'vue-router'
import Home from './Home/index.vue'
import PlcList from './PlcList/index.vue'
import PlcsDashboard from './PlcList/PlcsDashboard.vue'
import Plc from './Plc/index.vue'
import PlcLogs from './Plc/Logs/index.vue'
import PlcCreate from './Plc/PlcCreate.vue'
import System from './System/index.vue'
import PlcDebug from './Plc/Debug/index.vue'
import PlcDashboard from './Plc/Dashboard/index.vue'
import PlcStateLogs from './Plc/StateLogs/index.vue'
import PlcStateErrors from './Plc/StateErrors/index.vue'
import PlcConfig from './Plc/PlcConfig.vue'
import PlcStates from './PlcStates/index.vue'
import PlcState from './PlcStates/PlcState/index.vue'
import PlcStateCreate from './PlcStates/PlcState/StateCreate.vue'
import PlcStateDetail from './PlcStates/PlcState/StateDetail.vue'
import PlcStateHttpPushers from './PlcStates/PlcState/HttpPushers.vue'
import HttpPusherLogs from './Plc/HttpPusherLogs/index.vue'
import HttpPusherErrors from './Plc/HttpPusherErrors/index.vue'
import NotFound from './_public/NotFound.vue'
import UnlockSystem from './UnlockSystem.vue'

let routes: RouteConfig[] = [
  {
    path: '/',
    name: 'Home',
    redirect: '/plcs',
    component: Home,
    children: [
      {
        path: '/system',
        name: 'System',
        component: System
      },
      {
        path: 'plcs',
        component: PlcList,
        children: [
          {
            path: '',
            name: 'PlcsDashboard',
            component: PlcsDashboard
          },
          {
            path: 'create',
            name: 'PlcCreate',
            component: PlcCreate
          },
          {
            path: ':plcId',
            name: 'Plc',
            redirect: '/plcs/:plcId/dashboard',
            props: (route: any) => ({ plcId: +route.params.plcId }),
            component: Plc,
            children: [
              {
                path: 'dashboard',
                name: 'PlcDashboard',
                component: PlcDashboard
              },
              {
                path: 'debug',
                name: 'PlcDebug',
                component: PlcDebug
              },
              {
                path: 'config',
                name: 'PlcConfig',
                component: PlcConfig
              },
              {
                path: 'logs',
                name: 'PlcLogs',
                redirect: '/plcs/:plcId/logs/state-logs',
                component: PlcLogs,
                children: [
                  {
                    path: 'state-logs',
                    name: 'PlcStateLogs',
                    component: PlcStateLogs
                  },
                  {
                    path: 'state-errors',
                    name: 'PlcStateErrors',
                    component: PlcStateErrors
                  },
                  {
                    path: 'http-pusher-logs',
                    name: 'PlcHttpPusherLogs',
                    component: HttpPusherLogs
                  },
                  {
                    path: 'http-pusher-errors',
                    name: 'PlcHttpPusherErrors',
                    component: HttpPusherErrors
                  },
                ]
              },
              {
                path: 'states',
                name: 'PlcStates',
                component: PlcStates,
                children: [
                  {
                    path: 'create',
                    name: 'PlcStateCreate',
                    component: PlcStateCreate
                  },
                  {
                    path: ':stateId',
                    redirect: '/plcs/:plcId/states/:stateId/detail',
                    name: 'PlcState',
                    component: PlcState,
                    children: [
                      {
                        path: 'detail',
                        name: 'PlcStateDetail',
                        component: PlcStateDetail
                      },
                      {
                        path: 'http-posters',
                        name: 'PlcStateHttpPushers',
                        component: PlcStateHttpPushers
                      },
                    ]
                  },
                ]
              }
            ]
          }
        ]
      },
    ]
  },
  {
    path: '/login/:path',
    name: 'Login',
    props: (route) => ({
      path: atob(route.params.path)
    }),
    component: () => import('./Login.vue')
  },
  {
    path: '/unlock-system',
    name: 'UnlockSystem',
    component: UnlockSystem
  },
  {
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]

export default routes
