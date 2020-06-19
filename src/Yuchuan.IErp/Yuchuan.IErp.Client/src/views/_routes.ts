import Home from './Home/index.vue'
import NotFound from './_public/NotFound.vue'

export default [
  {
    path: '/',
    component: Home,
    children: [
      {
        path: 'chanjet-hkj',
        component: () => import('./ChanjetHkj/Home.vue'),
      },
      {
        path: 'chanjet-hkj/:orgCode/:bookCode',
        name: 'ChanjetHkjBook',
        props: (route: any) => ({
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
            props: (route: any) => ({
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
    path: '*',
    name: 'NotFound',
    component: NotFound,
  }
]
