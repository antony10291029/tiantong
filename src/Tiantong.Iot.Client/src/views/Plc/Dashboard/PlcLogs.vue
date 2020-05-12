<template>
  <div>
    <table class="table is-bordered is-fullwidth">
      <thead>
        <th>日志</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="log in logs.data" :key="log.id">
          <td>{{log.message}}</td>
          <td>{{log.created_at.split('.')[0].split('T')[1]}}</td>
        </tr>
      </tbody>
    </table>

    <div style="height: 1.25rem"></div>

    <Pagination
      v-bind="logs.meta"
      @change="changePage"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import axios from '@/providers/axios'
import { PlcLog } from '@/entities'

@Component({
  name: 'PlcLogs'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  isRunning!: boolean

  interval = 0

  logs = {
    meta: {
      page: 0,
      pageSize: 10,
      total: 0
    },
    data: [] as PlcLog[]
  }

  page = 1

  pageSize = 15

  async getLogs () {
    let response = await axios.post('/plcs/logs/paginate', {
      plc_id: this.plcId,
      page: this.page,
      pageSize: this.pageSize,
    })

    this.logs = response.data
  }

  async changePage (page: number) {
    this.page = page
    await this.getLogs()
  }

  created () {
    this.getLogs()
    this.interval = setInterval(() => {
      if (this.isRunning) {
        setTimeout(this.getLogs, 1000)
      }
    }, 1000)
  }

  beforeDestroy () {
    clearInterval(this.interval)
  }

  @Watch('isRunning')
  handleIsRunningChange (value: boolean) {
    this.getLogs()
  }

}
</script>
