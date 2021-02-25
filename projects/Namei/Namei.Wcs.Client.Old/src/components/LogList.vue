<template>
  <div class="box">
    <SearchField
      :isPending="isPending"
      @search="handleSearch"
    />

    <slot></slot>

    <table class="table is-fullwidth is-bordered">
      <thead>
        <th>运行记录</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="log in logs.data" :key="log.id">
          <td>{{log.message}}</td>
          <td>{{log.created_at.split('T').join(' ')}}</td>
        </tr>
      </tbody>
    </table>

    <div style="height: 1.5rem"></div>

    <Pagination
      v-bind="logs.meta"
      @change="getDataSource"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'LifterLogs'
})
export default class extends Vue {
  @Prop({ required: true })
  search!: string[]

  query = ''

  isPending = false

  logs = {
    data: [],
    meta: {
      page: 1,
      pageSize: 15,
      total: 1
    }
  }

  async getDataSource (page = 1) {
    const response = await domain.post('/logs/search', {
      page,
      search: this.search,
      query: [this.query]
    })

    this.logs = response.data
  }

  async handleSearch (query: string) {
    try {
      this.isPending = true
      this.query = query
      await this.getDataSource()
    } finally {
      this.isPending = false
    }
  }

  interval: any

  created () {
    this.getDataSource()
  }

  @Watch('logs.meta.page', { immediate: true })
  handlePage (page: number) {
    if (page === 1) {
      this.interval = setInterval(this.getDataSource, 1000)
    } else {
      clearInterval(this.interval)
    }
  }

  beforeDestroy () {
    if (this.interval) {
      clearInterval(this.interval)
    }
  }
}
</script>
