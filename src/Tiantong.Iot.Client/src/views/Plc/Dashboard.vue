<template>
  <div style="padding: 1.25rem">
    <div class="is-flex">
      <AsyncButton
        :handler="getDataSource"
        class="button is-info is-light is-small"
      >
        <span class="iconfont">
          <i class="iconfont icon-refresh"></i>
        </span>
      </AsyncButton>

      <span style="width: 0.5rem"></span>

      <PlcRunningButton
        :plcId="plcId"
        :isRunning="isRunning"
        @change="isRunning = $event"
      />
    </div>

    <div style="height: 0.75rem"></div>

    <div class="columns">
      <div class="column">
        <PlcStates
          :plcId="plcId"
          :isRunning="isRunning"
        />
      </div>

      <div class="column">
        <PlcLogs :plcId="plcId" :isRunning="isRunning" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcRunningButton from './PlcRunningButton.vue'
import PlcLogs from './PlcLogs.vue'
import PlcStates from './PlcStates.vue'
import axios from '@/providers/axios'

@Component({
  name: 'PlcDashboard',
  components: {
    PlcLogs,
    PlcStates,
    PlcRunningButton,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  isRunning = false

  async getDataSource () {
    let response = await axios.post('/plcs/workers/is-running', {
      plc_id: this.plcId
    })

    this.isRunning = response.data.is_running
  }

  created () {
    this.getDataSource()
  }
}
</script>
