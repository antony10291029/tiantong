<template>
  <div>
    <table class="table is-bordered is-fullwidth">
      <thead>
        <th>日志</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="log in logs" :key="log.id">
          <td>{{log.message}}</td>
          <td>{{log.created_at.split('.')[0].split('T')[1]}}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcLogs'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  isRunning!: boolean

  logs: any[] = []

  async getLogs () {
    let response = await axios.post('/plcs/logs/paginate', {
      plc_id: this.plcId,
      page: 1,
      pageSize: 20,
    })

    this.logs = response.data.data
  }

  created () {
    this.getLogs()
    setInterval(() => {
      if (this.isRunning) {
        setTimeout(this.getLogs, 1000)
      }
    }, 1000)
  }

  @Watch('isRunning')
  handleIsRunningChange (value: boolean) {
    if (value === false) {
      this.getLogs()
    }
  }

}
</script>
