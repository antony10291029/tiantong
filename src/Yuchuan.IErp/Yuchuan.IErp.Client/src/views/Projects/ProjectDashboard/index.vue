<template>
  <div style="padding: 1.25rem; overflow: auto">
    <Chart
      ref="chart"
      :states="states.data"
      :devices="devices.list.map(id => devices.data[id])"
    ></Chart>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import * as signalR from '@microsoft/signalr'
import Chart from './Chart.vue'

@Component({
  name: 'ProjectDashboard',
  components: {
    Chart
  }
})
export default class extends Vue {
  @Prop({ required: true })
  projectId!: number

  states = {
    data: {} as any,
    list: []
  }

  devices = {
    data: {} as any,
    list: []
  }

  created () {
    let hub = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.VUE_APP_API_URL}/device-states`)
      .withAutomaticReconnect()
      .build()

    hub.on('commit', (state: any) => {
      this.states.data[state.device_id][state.key] = state.value
    })

    hub.on('initialize', dataSource => {
      let data: any = {}, list: any = []

      dataSource.devices.forEach((device: any) => {
        data[device.id] = device
        list.push(device.id)
      })
      this.devices = { data, list }

      data = {}
      list = []
      dataSource.states.forEach((state: any) => {
        data[state.device_id] = state
        list.push(state.id)
      })
      this.states = { data, list };
    })

    hub.start().then(() => {
      hub.send('useProject', this.projectId)
    })

    this.$once('hook:destroyed', () => {
      hub.stop()
    })
  }
}
</script>
