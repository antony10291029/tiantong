import Home from './Home/index.vue'
import NotFound from './_public/NotFound.vue'
import { RouteConfig } from 'vue-router'

const routes: RouteConfig[] = [
  {
    path: '/',
    component: Home,
    children: [
      {
        path: '/projects',
        name: 'Projects',
        meta: { login: true },
        component: () => import('./Projects/index.vue'),
        children: [
          {
            path: 'create',
            name: 'CreateProject',
            component: () => import('./Projects/CreateProject.vue')
          },
          {
            path: ':projectId',
            name: 'ProjectChart',
            redirect: '/projects/:projectId/dashboard',
            props: route => ({
              projectId: +route.params.projectId
            }),
            component: () => import('./Projects/Project.vue'),
            children: [
              {
                path: 'dashboard',
                name: 'ProjectDashboard',
                component: () => import('./Projects/ProjectDashboard/index.vue')
              },
              {
                path: 'detail',
                name: 'ProjectDetail',
                component: () => import('./Projects/ProjectDetail.vue')
              },
              {
                path: 'devices',
                name: 'ProjectDevices',
                component: () => import('./Projects/Devices.vue'),
                children: [
                  {
                    path: 'create',
                    name: 'CreateDevice',
                    component: () => import('./Projects/CreateDevice.vue')
                  },
                  {
                    path: ':deviceId',
                    name: 'DeviceDetail',
                    props: route => ({
                      deviceId: +route.params.deviceId
                    }),
                    component: () => import('./Projects/DeviceDetail.vue')
                  }
                ]
              }
            ]
          }
        ]
      },
      {
        path: 'chanjet-hkj',
        component: () => import('./ChanjetHkj/Home.vue'),
      },
      {
        path: 'chanjet-hkj/:orgCode/:bookCode',
        name: 'ChanjetHkjBook',
        props: route => ({
          orgCode: route.params.orgCode,
          bookCode: route.params.bookCode
        }),
        component: () => import('./ChanjetHkj/Book.vue'),
        children: [
          {
            path: 'settings',
            name: 'HkjSettings',
            component: () => import('./ChanjetHkj/Settings.vue')
          },
          {
            path: ':projectCode',
            name: 'ProjectHome',
            props: route => ({
              projectCode: decodeURIComponent(atob(route.params.projectCode))
            }),
            component: () => import('./ChanjetHkj/Project.vue')
          },
        ]
      }
    ]
  },
  {
    path: '/chanjet-login',
    name: 'ChanjetLogin',
    props: (route: any) => ({
      redirect: route.params.redirect,
    }),
    component: () => import('./ChanjetHkj/Login.vue'),
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
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]

export default routes
