<template>
  <AsyncButton
    v-if="!isRunning"
    :handler="handleRun"
    class="button is-small is-info is-light"
  >
    <span>运行</span>
    <span class="icon is-small">
      <i class="iconfont icon-play"></i>
    </span>
  </AsyncButton>
  <AsyncButton
    v-else
    :handler="handleStop"
    class="button is-small is-info is-light"
  >
    <span>停止</span>
    <span class="icon is-small">
      <i class="iconfont icon-stop"></i>
    </span>
  </AsyncButton>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcRunningButton'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  isRunning!: boolean

  async handleRun () {
    await axios.post('/plcs/workers/run', {
      plc_id: this.plcId
    })

    this.$emit('change', true)
  }

  async handleStop () {
    await axios.post('/plcs/workers/stop', {
      plc_id: this.plcId
    })

    this.$emit('change', false)
  }
}
</script>
