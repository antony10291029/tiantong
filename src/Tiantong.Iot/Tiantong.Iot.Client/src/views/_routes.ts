import { RouteConfig } from 'vue-router'
import Home from './Home/index.vue'
import Login from './Login/index.vue'
import NotFound from './NotFound/index.vue'
import Plcs from './Plcs/index.vue'
import PlcsCreate from './Plcs.Create/index.vue'
import PlcsDashboard from './Plcs.Dashboard/index.vue'
import PlcsPlc from './Plcs.Plc/index.vue'
import PlcsPlcDashboard from './Plcs.Plc.Dashboard/index.vue'
import PlcsPlcDebug from './Plcs.Plc.Debug/index.vue'
import PlcsPlcDetail from './Plcs.Plc.Detail/index.vue'
import PlcsPlcLogs from './Plcs.Plc.Logs/index.vue'
import PlcsPlcLogsHttpPusher from './Plcs.Plc.Logs.HttpPusher/index.vue'
import PlcsPlcLogsHttpPusherErrors from './Plcs.Plc.Logs.HttpPusherErrors/index.vue'
import PlcsPlcLogsStateLogs from './Plcs.Plc.Logs.StateLogs/index.vue'
import PlcsPlcLogsStateErrors from './Plcs.Plc.Logs.StateErrors/index.vue'
import PlcsPlcStatesCreate from './Plcs.Plc.States.Create/index.vue'
import PlcsPlcStatesIndex from './Plcs.Plc.States.Index/index.vue'
import PlcsPlcStatesState from './Plcs.Plc.States.State/index.vue'
import PlcsPlcStatesStateDetail from './Plcs.Plc.States.State.Detail/index.vue'
import PlcsPlcStatesStateHttpPusher from './Plcs.Plc.States.State.HttpPushers/index.vue'
import System from './System/index.vue'
import UnlockSystem from './UnlockSystem/index.vue'

const routes: RouteConfig[] = [
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
        component: Plcs,
        children: [
          {
            path: '',
            name: 'PlcsDashboard',
            component: PlcsDashboard
          },
          {
            path: 'create',
            name: 'PlcsCreate',
            component: PlcsCreate
          },
          {
            path: ':plcId',
            name: 'PlcsPlc',
            redirect: '/plcs/:plcId/dashboard',
            props: route => ({ plcId: +route.params.plcId }),
            component: PlcsPlc,
            children: [
              {
                path: 'dashboard',
                name: 'PlcsPlcDashboard',
                component: PlcsPlcDashboard
              },
              {
                path: 'debug',
                name: 'PlcsPlcDebug',
                component: PlcsPlcDebug
              },
              {
                path: 'config',
                name: 'PlcsPlcDetail',
                component: PlcsPlcDetail
              },
              {
                path: 'logs',
                name: 'PlcsPlcLogs',
                redirect: '/plcs/:plcId/logs/state-logs',
                component: PlcsPlcLogs,
                children: [
                  {
                    path: 'state-logs',
                    name: 'PlcsPlcLogsStateLogs',
                    component: PlcsPlcLogsStateLogs
                  },
                  {
                    path: 'state-errors',
                    name: 'PlcsPlcLogsStateErrors',
                    component: PlcsPlcLogsStateErrors
                  },
                  {
                    path: 'http-pusher-logs',
                    name: 'PlcsPlcLogsHttpPusher',
                    component: PlcsPlcLogsHttpPusher
                  },
                  {
                    path: 'http-pusher-errors',
                    name: 'PlcPlcsPlcLogsHttpPusherErrors',
                    component: PlcsPlcLogsHttpPusherErrors
                  },
                ]
              },
              {
                path: 'states',
                name: 'PlcsPlcStatesIndex',
                component: PlcsPlcStatesIndex,
                children: [
                  {
                    path: 'create',
                    name: 'PlcsPlcStatesCreate',
                    component: PlcsPlcStatesCreate
                  },
                  {
                    path: ':stateId',
                    redirect: '/plcs/:plcId/states/:stateId/detail',
                    name: 'PlcsPlcStatesState',
                    component: PlcsPlcStatesState,
                    children: [
                      {
                        path: 'detail',
                        name: 'PlcsPlcStatesStateDetail',
                        component: PlcsPlcStatesStateDetail
                      },
                      {
                        path: 'http-posters',
                        name: 'PlcsPlcStatesStateHttpPusher',
                        component: PlcsPlcStatesStateHttpPusher
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
    component: Login
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
