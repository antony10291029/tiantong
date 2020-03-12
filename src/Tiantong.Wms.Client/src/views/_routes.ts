const routes = [
  {
    path: '/',
    redirect: '/warehouses'
  },
  {
    path: '/home',
    redirect: '/warehouses'
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('./Public/Register.vue'),
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('./Public/Login.vue'),
  },
  {
    path: '/unauthorization',
    name: 'Unthorization',
    component: () => import('./Public/Unauthorization.vue')
  },
  {
    path: '/',
    name: 'Home',
    alias: '/warehouses',
    component: () => import('./Home/index.vue'),
    children: [
      {
        path: '/setting',
        name: 'Setting',
        component: () => import('./Setting/index.vue')
      },
      {
        path: 'warehouses',
        name: 'Warehouses',
        component: () => import('./Warehouses/index.vue')
      },
      {
        path: '/warehouses/create',
        name: 'CreateWarehouse',
        component: () => import('./WarehouseCreate/index.vue')
      },
      {
        path: '/warehouses/:id',
        name: 'Warehouse',
        component: () => import('./Warehouse/index.vue')
      }
    ]
  },
  {
    path: '*',
    name: 'NotFound',
    component: () => import('./Public/NotFound.vue')
  }
]

export default routes
