<template>
  <div style="padding: 1.25rem">
    <p class="title is-5">
      设备控制
    </p>

    <p class="content">
      该操作将一键运行或停止当前系统中的所有设备
    </p>

    <AsyncButton
      :handler="run"
      style="margin-right: 0.5rem"
      class="button is-info is-light is-small"
    >
      运行
    </AsyncButton>

    <AsyncButton
      :handler="stop"
      class="button is-info is-light is-small"
    >
      停止
    </AsyncButton>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'StopDevices'
})
export default class extends Vue {
  async run() {
    await axios.post('/plc-workers/start-all')
  }

  async stop () {
    await axios.post('/plc-workers/stop-all')
  }
}
</script>
