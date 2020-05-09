import Home from './Home/index.vue'
import Plc from './Plc/index.vue'
import PlcList from './PlcList/index.vue'
import PlcState from './PlcState/index.vue'
import PlcStates from './PlcStates/index.vue'
import PlcStateCreate from './PlcState/StateCreate.vue'
import PlcStateDetail from './PlcState/StateDetail.vue'
import PlcStateHttpPushers from './PlcState/HttpPushers.vue'
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
            path: ':plcId',
            name: 'Plc',
            redirect: '/plcs/:plcId/states',
            props: (route: any) => ({ plcId: +route.params.plcId }),
            component: Plc,
            children: [
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
