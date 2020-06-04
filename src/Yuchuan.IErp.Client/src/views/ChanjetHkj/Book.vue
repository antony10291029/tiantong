<template>
  <AsyncLoader :handler="getData">
    <aside
      class="menu has-border-right is-unselectable"
      style="min-width: 220px; max-width: 220px; height: 100%; overflow-y: auto"
    >
      <ul class="menu-list">
        <li v-for="book in books" :key="book.bookCode">
          <router-link
            :to="`/chanjet-hkj/${orgCode}/${book.bookCode}`"
            active-class="false"
            exact-active-class="is-active"
          >
            <span class="icon">
              <i class="iconfont icon-account-book"></i>
            </span>
            <span>
              {{book.name}}
            </span>
          </router-link>
        </li>
        <li>
          <router-link :to="baseURL + '/settings'">
            <span class="icon">
              <i class="iconfont icon-settings"></i>
            </span>
            <span>
              报表配置
            </span>
          </router-link>
        </li>
        <li>
          <a @click="handleLogout">
            <span class="icon">
              <i class="iconfont icon-logout"></i>
            </span>
            <span>
              退出登陆
            </span>
          </a>
        </li>
      </ul>
    </aside>

    <div
      class="is-flex is-flex-auto"
      style="padding: 1.25rem; overflow: auto"
    >
      <div style="width: 320px; margin-right: 1.25rem">
        <nav class="panel">
          <p
            class="panel-heading is-size-6"
            style="padding: 0.75rem"
          >
            工程
          </p>
          <router-link
            v-for="project in projects" :key="project.no"
            class="panel-block"
            style="padding: 0.25rem 0.75rem"
            :to="`${baseURL}/${base64Encode(project.no)}`"
          >
            <div
              class="is-flex is-flex-column"
            >
              <span>{{project.name}}</span>
              <span>{{project.no}}</span>
            </div>
          </router-link>
        </nav>
      </div>

      <router-view
        :orgCode="orgCode"
        :bookCode="bookCode"
        :key="$route.params.projectCode"
        class="is-flex-auto"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import localStorage from '@/providers/local-storage'
import chanjetContext from '@/providers/contexts/chanjet'

@Component({
  name: 'ChanjetBookDashboard',
})
export default class extends Vue {
  @Prop({ required: true })
  orgCode!: string

  @Prop({ required: true })
  bookCode!: string

  books = [] as any[]

  projects = [] as any[]

  details = [] as any[]

  get baseURL () {
    return `/chanjet-hkj/${this.orgCode}/${this.bookCode}`
  }

  handleLogout () {
    this.$confirm({
      title: '提示',
      content: '退出后需要重新登陆',
      handler: () => {
        localStorage.chanjet.clear()
        this.$router.push('/')
        this.$notify.info('好会计账户已退出')
      }
    })
  }

  base64Encode (text: string) {
    return btoa(encodeURIComponent(text))
  }

  async getData () {
    this.books = await chanjetContext.getBooks(this.orgCode, this.bookCode)
    this.projects = await chanjetContext.getProjects(this.orgCode, this.bookCode)
  }
}
</script>
