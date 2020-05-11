<template>
  <AsyncLoader
    class="is-flex-auto"
    :handler="getStates"
  >
    <table class="table is-fullwidth is-bordered">
      <thead>
        <th>数据名</th>
        <th>地址</th>
        <th>数据</th>
        <th>时间</th>
      </thead>
      <tbody>
        <tr v-for="state in states" :key="state.id">
          <td>{{state.name}}</td>
          <td>{{state.address}}</td>
          <td></td>
          <td></td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcDashboardStates'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  states: any[] = []

  async getStates () {
    let response = await axios.post('plcs/states/all', {
      plc_id: this.plcId
    })

    this.states = response.data
  }
}
</script>
