import Home from './Home/index.vue'
import Plc from './Plc/index.vue'
import PlcList from './PlcList/index.vue'
import PlcStates from './PlcStates/index.vue'
import NotFound from './_public/NotFound.vue'

export default [
  {
    path: '/',
    alias: '/plcs',
    name: 'Home',
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
            props: (route: any) => ({ plcId: +route.params.plcId }),
            component: Plc,
            children: [
              {
                path: '',
                name: 'PlcStates',
                component: PlcStates
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
