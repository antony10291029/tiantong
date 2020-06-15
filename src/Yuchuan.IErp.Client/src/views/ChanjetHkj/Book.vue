<template>
  <AsyncLoader :handler="getData">
    <aside class="menu-container">
      <div
        class="menu is-unselectable"
        style="min-width: 260px; max-width: 260px"
      >
        <ul class="menu-list">
          <li v-for="book in books" :key="book.bookCode">
            <router-link :to="`/chanjet-hkj/${orgCode}/${book.bookCode}`">
              <span class="icon">
                <i class="iconfont icon-account-book"></i>
              </span>
              <span>
                {{book.name}}
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
      </div>
    </aside>

    <div
      class="is-flex-auto is-flex-column"
      style="overflow: auto"
    >
      <div v-if="false" class="tabs" style="margin-bottom: 0;">
        <ul>
          <li class="is-active">
            <a>
              <span>工程统计</span>
            </a>
          </li>
        </ul>
      </div>
      <div
        class="is-flex-auto"
        style="padding: 1.5rem; overflow: auto"
      >
        <div class="columns is-variable is-3">
          <div
            class="column is-narrow"
            style="width: 320px; min-width: 320px"
          >
            <nav class="panel">
              <p class="panel-heading">
                工程
              </p>
              <router-link
                class="panel-block"
                v-for="project in projects" :key="project.no"
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
            :categories="categories"
            :key="$route.params.projectCode"
          />
        </div>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import localStorage from '@/providers/local-storage'
import chanjetContext from '@/providers/contexts/chanjet'
import domainContext from '@/providers/contexts/domain'

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

  categories = [] as any[]

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

  async getCategories () {
    const response = await domainContext.post('/subject-categories/search', {
      book_code: this.bookCode
    })

    return response.data
  }

  async getData () {
    this.books = await chanjetContext.getBooks(this.orgCode, this.bookCode)
    this.projects = await chanjetContext.getProjects(this.orgCode, this.bookCode)
    this.categories = await this.getCategories()
  }
}
</script>
