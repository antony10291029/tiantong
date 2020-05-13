<template>
  <AsyncLoader
    class="is-flex-auto"
    :handler="getStates"
  >
    <table class="table is-centered is-fullwidth is-bordered">
      <thead>
        <th>数据名</th>
        <th>地址</th>
        <th>数据</th>
        <th>操作</th>
      </thead>
      <tbody>
        <tr v-for="state in states" :key="state.id">
          <td>{{state.name}}</td>
          <td>{{state.address}}</td>
          <td>{{currentValues[state.id]}}</td>
          <PlcStateSetValue
            v-if="isRunning"
            :plcId="plcId"
            :stateId="state.id"
            :type="state.type"
          />
          <td v-else></td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import PlcStateSetValue from './PlcStateSetValue.vue'

@Component({
  name: 'PlcDashboardStates',
  components: {
    PlcStateSetValue
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  isRunning!: boolean

  @Prop({ required: true })
  isDataWatchOpen!: boolean

  interval = 0

  states: any[] = []

  currentValues: { [ key: string ]: string } = {}

  async getStates () {
    let response = await axios.post('plcs/states/all', {
      plc_id: this.plcId
    })

    this.states = response.data
  }

  async getCurrentVales () {
    let response = await axios.post('/plcs/workers/current-values', {
      plc_id: this.plcId
    })

    this.currentValues = response.data
  }

  created () {
    this.interval = setInterval(() => {
      if (this.isDataWatchOpen) {
        this.getCurrentVales()
      }
    }, 1000)
  }

  beforeDestroy () {
    clearInterval(this.interval)
  }
}
</script>
