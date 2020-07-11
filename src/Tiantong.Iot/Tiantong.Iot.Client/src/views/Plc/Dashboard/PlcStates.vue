<template>
  <AsyncLoader
    class="is-flex-auto box"
    :handler="getStates"
  >
    <Table
      colspan="5"
      class="table is-centered is-nowrap is-fullwidth"
    >
      <thead slot="head">
        <th>数据名</th>
        <th>地址</th>
        <th>类型</th>
        <th>数据</th>
        <th style="width: 1px">操作</th>
      </thead>
      <tbody
        slot="body"
        v-if="states.length"
      >
        <tr v-for="state in states" :key="state.id">
          <td>{{state.name}}</td>
          <td>{{state.address}}</td>
          <td>
            {{state.type + (state.type === 'string' ? `(${state.length * 2})` : '')}}
          </td>
          <td>{{currentValues[state.id]}}</td>
          <PlcStateSetValue
            v-if="isRunning"
            :plcId="plcId"
            :stateId="state.id"
            :type="state.type"
            style="width: 1px"
          />
          <td v-else></td>
        </tr>
      </tbody>
    </Table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcStateSetValue from './PlcStateSetValue.vue'
import axios from '@/providers/axios'

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
    let response = await axios.post('/plc-workers/current-values', {
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