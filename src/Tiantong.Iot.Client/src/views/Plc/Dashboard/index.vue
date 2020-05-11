<template>
  <div style="padding: 1.25rem">
    <div class="is-flex is-vcentered">
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
        @change="handleRunningChange"
      />

      <span style="width: 0.5rem"></span>

      <Checkbox
        v-if="isRunning"
        v-model="isDataWatchOpen"
      >实时数据</Checkbox>
    </div>

    <div style="height: 0.75rem"></div>

    <div
      v-if="isRunning"
      class="columns"
    >
      <div class="column">
        <PlcStates
          :plcId="plcId"
          :isRunning="isRunning"
          :isDataWatchOpen="isDataWatchOpen"
        />
      </div>

      <div class="column">
        <PlcLogs
          :plcId="plcId"
          :isRunning="isRunning"
        />
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

  isDataWatchOpen = false

  async getDataSource () {
    let response = await axios.post('/plcs/workers/is-running', {
      plc_id: this.plcId
    })

    this.handleRunningChange(response.data.is_running)
  }

  handleRunningChange (isRunning: boolean) {
    if (isRunning === false) {
      this.isDataWatchOpen = false
    } else {
      this.isDataWatchOpen = true
    }

    this.isRunning = isRunning
  }

  created () {
    this.getDataSource()
  }
}
</script>
