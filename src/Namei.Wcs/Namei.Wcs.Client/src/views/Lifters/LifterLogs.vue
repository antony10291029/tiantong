<template>
  <div class="box">
    <table class="table is-fullwidth">
      <thead>
        <th>运行记录</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="log in logs.data" :key="log.id">
          <td>{{log.message}}</td>
          <td>{{log.created_at.split('T')[1]}}</td>
        </tr>
      </tbody>
    </table>

    <div style="height: 1.25rem"></div>

    <Pagination
      v-bind="logs.meta"
      @change="getDataSource"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'LifterLogs'
})
export default class extends Vue {
  logs = {
    data: [],
    meta: {
      page: 1,
      pageSize: 15,
      total: 1
    }
  }

  async getDataSource (page = 1) {
    const response = await domain.post('/lifters/logs', {
      page
    })

    this.logs = response.data
  }

  interval: any

  @Watch('logs.meta.page', { immediate: true })
  handlePage (page: number) {
    if (page === 1) {
      this.interval = setInterval(this.getDataSource, 1000)
    } else {
      clearInterval(this.interval)
    }
  }

  created () {
    this.getDataSource()
  }
}
</script>
