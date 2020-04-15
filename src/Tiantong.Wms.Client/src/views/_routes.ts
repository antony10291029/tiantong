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
        redirect: '/warehouses/:warehouseId/purchase-orders/',
        name: 'Warehouse',
        props: (route: any) => ({ warehouseId: +route.params.warehouseId }),
        component: () => import('./Warehouse/index.vue'),
        children: [
          {
            path: 'departments',
            name: 'Departments',
            component: () => import('./Departments/DepartmentList.vue'),
            children: [
              {
                path: 'create',
                name: 'DepartmentCreate',
                component: () => import('./Departments/DepartmentCreate.vue')
              },
              {
                path: ':departmentId/update',
                name: 'DepartmentUpdate',
                props: (route: any) =>  ({ departmentId: +route.params.departmentId }),
                component: () => import('./Departments/DepartmentUpdate.vue')
              }
            ]
          },
          {
            path: 'users',
            name: 'WarehouseUserList',
            component: () => import('./WarehouseUsers/WarehouseUserList.vue'),
            children: [
              {
                path: 'create',
                name: 'WarehouseUserCreate',
                component: () => import('./WarehouseUsers/WarehouseUserCreate.vue')
              },
              {
                path: ':warehouseUserId/update',
                name: 'WarehouseUserUpdate',
                props: (route: any) => ({ warehouseUserId: +route.params.warehouseUserId }),
                component: () => import('./WarehouseUsers/WarehouseUserUpdate.vue')
              }
            ]
          },
          {
            path: 'suppliers',
            name: 'Suppliers',
            component: () => import('./Suppliers/index.vue'),
            children: [
              {
                path: 'create',
                name: 'SupplierCreate',
                component: () => import('./Suppliers/SupplierCreate.vue')
              },
              {
                path: ':supplierId/update',
                name: 'SupplierUpdate',
                props: (route: any) => ({ supplierId: +route.params.supplierId }),
                component: () => import('./Suppliers/SupplierUpdate.vue')
              },
            ]
          },
          {
            path: 'projects',
            name: 'Projects',
            component: () => import('./Projects/index.vue'),
            children: [
              {
                path: 'create',
                name: 'ProjectCreate',
                component: () => import('./Projects/ProjectCreate.vue')
              },
              {
                path: ':projectId/update',
                name: 'ProjectUpdate',
                props: (route: any) => ({ projectId: +route.params.projectId }),
                component: () => import('./Projects/ProjectUpdate.vue')
              }
            ]
          },
          {
            path: 'settings',
            name: 'WarehouseSettings',
            component: () => import('./Warehouse/WarehouseSettings.vue')
          },
          {
            path: 'areas',
            name: 'Areas',
            component: () => import('./Areas/index.vue')
          },
          {
            path: 'goods',
            component: () => import('./Goods/index.vue'),
            children: [
              {
                path: '',
                name: 'GoodList',
                component: () => import('./Goods/GoodList.vue')
              },
              {
                path: 'create',
                name: 'GoodCreate',
                component: () => import('./Goods/GoodCreate.vue')
              },
              {
                path: ':goodId/manage',
                name: 'GoodManage',
                props: (route: any) => ({ goodId: +route.params.goodId }),
                component: () => import('./Goods/GoodManage.vue'),
                children: [
                  {
                    path: 'items/add',
                    name: 'GoodItemsAdd',
                    component: () => import('./Goods/GoodItemsAdd.vue')
                  }
                ]
              },
            ]
          },
          {
            path: 'purchase-orders',
            name: 'PurchaseOrders',
            component: () => import('./PurchaseOrders/index.vue'),
            children: [
              {
                path: '',
                name: 'PurchaseOrderList',
                component: () => import('./PurchaseOrders/OrderList/index.vue')
              },
              {
                path: 'create',
                name: 'PurchaseOrderCreate',
                component: () => import('./PurchaseOrders/PurchaseOrderCreate.vue')
              },
              {
                path: ':orderId/detail',
                name: 'PurchaseOrderDetail',
                props: (route: any) => ({ orderId: + route.params.orderId }),
                component: () => import('./PurchaseOrders/PurchaseOrderDetail.vue')
              }
            ]
          },
          {
            path: 'pickings',
            name: 'Pickings',
            component: () => import('./Pickings/index.vue')
          },
          {
            path: 'locations',
            name: 'Locations',
            component: () => import('./Locations/index.vue')
          },
          {
            path: 'inventory',
            name: 'Inventory',
            component: () => import('./Inventory/index.vue')
          },
          {
            path: 'item-categories',
            name: 'ItemCategories',
            component: () => import('./ItemCategories/index.vue'),
            children: [
              {
                path: 'create',
                name: 'CreateItemCategory',
                component: () => import('./ItemCategories/ItemCategoryCreate.vue')
              },
              {
                path: ':categoryId/update',
                name: 'UpdateItemCategory',
                props: (route: any) => ({ categoryId: +route.params.categoryId }),
                component: () => import('./ItemCategories/ItemCategoryUpdate.vue')
              }
            ]
          },
        ]
      }
    ]
  },
  {
    path: '/test',
    name: 'Test',
    component: () => import('./Public/Test.vue')
  },
  {
    path: '*',
    name: 'NotFound',
    component: () => import('./Public/NotFound.vue')
  }
]

export default routes
