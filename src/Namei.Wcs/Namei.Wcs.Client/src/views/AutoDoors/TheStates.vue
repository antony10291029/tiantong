<template>
  <AsyncLoader
    :handler="getStates"
    class="box" style="width: 400px"
  >
    <table class="table is-fullwidth">
      <thead>
        <th>编号</th>
        <th>开关状态</th>
      </thead>
      <tbody>
        <tr v-for="state in states" :key="state.id">
          <td>{{state.id}}</td>
          <td>
            <span
              v-if="state.isClosed"
              class="tag is-success"
            >
              已关闭
            </span>
            <span
              v-else
              class="tag is-warning"
            >
              已打开
            </span>
          </td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'AutoDoorStates'
})
export default class extends Vue {
  interval: any

  states: any[] = []

  async getStates () {
    const response = await domain.post('/doors/states')

    this.states = response.data
  }

  created () {
    this.interval = setInterval(this.getStates, 1000)
  }

  beforeDestroy () {
    clearInterval(this.interval)
  }
}
</script>
