<template>
  <AsyncLoader
    :handler="getStates"
    class="box"
  >
    <table class="table is-fullwidth is-centered is-nowrap">
      <thead>
        <th>编号</th>
        <th>类型</th>
        <th>开关状态</th>
        <th>允许进入</th>
        <th>等待中的任务</th>
        <th>任务数量</th>
      </thead>
      <tbody>
        <tr v-for="state in states" :key="state.id">
          <td>{{state.id}}</td>
          <td>{{state.type}}</td>
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
          <td>
            <span
              v-if="state.avaliable"
              class="tag is-success"
            >
              允许
            </span>
            <span
              v-else
              class="tag is-warning"
            >
              禁止
            </span>
          </td>
          <td>
            {{state.taskId}}
          </td>
          <td>
            {{state.count}}
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
