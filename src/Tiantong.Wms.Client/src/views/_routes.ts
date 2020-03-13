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
        path: '/settings',
        name: 'Setting',
        component: () => import('./WarehouseSettings/index.vue')
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
        path: '/warehouses/:warehouseId',
        name: 'Warehouse',
        props: (route: any) => ({ warehouseId: +route.params.warehouseId }),
        component: () => import('./Warehouse/index.vue'),
        children: [
          {
            path: 'suppliers',
            name: 'Suppliers',
            component: () => import('./Suppliers/index.vue')
          },
          {
            path: 'projects',
            name: 'Projects',
            component: () => import('./Projects/index.vue')
          },
          {
            path: 'settings',
            name: 'WarehouseSettings',
            component: () => import('./WarehouseSettings/index.vue')
          },
          {
            path: 'areas',
            name: 'Areas',
            component: () => import('./Areas/index.vue')
          },
          {
            path: 'stocks',
            name: 'Stocks',
            component: () => import('./Stocks/index.vue')
          },
          {
            path: 'pickings',
            name: 'Pickings',
            component: () => import('./Pickings/index.vue')
          },
          {
            path: 'inbounds',
            name: 'Inbounds',
            component: () => import('./Inbounds/index.vue')
          },
          {
            path: 'returns',
            name: 'Returns',
            component: () => import('./Returns/index.vue')
          },
          {
            path: 'locations',
            name: 'Locations',
            component: () => import('./Locations/index.vue')
          },
          {
            path: 'inbounds',
            name: 'Inbounds',
            component: () => import('./Inbounds/index.vue')
          },
          {
            path: 'inventory',
            name: 'Inventory',
            component: () => import('./Inventory/index.vue')
          },
          {
            path: 'item-categories',
            name: 'ItemCategories',
            component: () => import('./ItemCategories/index.vue')
          },
        ]
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
