import Home from './Home/index.vue'
import Plc from './Plc/index.vue'
import PlcCreate from './Plc/PlcCreate.vue'
import PlcDashboard from './Plc/Dashboard/index.vue'
import PlcList from './PlcList/index.vue'
import PlcConfig from './Plc/PlcConfig.vue'
import PlcStates from './PlcStates/index.vue'
import PlcState from './PlcStates/PlcState/index.vue'
import PlcStateCreate from './PlcStates/PlcState/StateCreate.vue'
import PlcStateDetail from './PlcStates/PlcState/StateDetail.vue'
import PlcStateHttpPushers from './PlcStates/PlcState/HttpPushers.vue'
import NotFound from './_public/NotFound.vue'

export default [
  {
    path: '/',
    name: 'Home',
    redirect: '/plcs',
    component: Home,
    children: [
      {
        path: 'plcs',
        name: 'PlcList',
        component: PlcList,
        children: [
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
                path: 'config',
                name: 'PlcConfig',
                component: PlcConfig
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
                      }
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
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]
