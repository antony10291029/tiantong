<template>
  <div style="padding: 1.25rem">
    <AsyncButton
      :handler="handlePut"
      class="button is-info"
    >
      1F 放货完成
    </AsyncButton>

    <div style="height: 1.25rem"></div>

    <AsyncButton
      :handler="handleExecute"
      class="button is-info"
    >
      1F 执行任务
    </AsyncButton>

  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import axios from '@/providers/contexts/_axios'

const context = axios.create('http://172.16.2.94:5000')

@Component({
  name: 'Lifter'
})
export default class extends Vue {
  async handlePut () {
    await context.post('/plc-states/uint16/set', {
      plc: '货梯改造',
      state: '1F - A 段输送机 - AGC 状态',
      value: 128
    })
  }

  async handleExecute () {
    await context.post('/plc-states/uint16/set', {
      plc: '货梯改造',
      state: '1F - A 段输送机 - 货物终点',
      value: 2
    })
  }
}
</script>
