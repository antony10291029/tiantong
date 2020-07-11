<template>
  <div style="padding: 1.25rem">
    <div class="is-flex is-vcentered">
      <PlcRunningButton
        :plcId="plcId"
        :isRunning="isRunning"
        @change="handleRunningChange"
      />

      <span style="width: 0.5rem"></span>

      <Checkbox
        v-if="isRunning"
        v-model="isDataWatchOpen"
      >
        实时监控
      </Checkbox>
    </div>

    <div style="height: 1.25rem"></div>

      <template v-if="currentTab == 0">
        <div class="columns">
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
      </template>
      <template v-else-if="currentTab == 1">
        <PlcDataDebug />
      </template>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcRunningButton from './PlcRunningButton.vue'
import PlcLogs from './PlcLogs.vue'
import PlcStates from './PlcStates.vue'
import PlcDataDebug from './DataDebug.vue'
import axios from '@/providers/axios'

@Component({
  name: 'PlcDashboard',
  components: {
    PlcLogs,
    PlcStates,
    PlcDataDebug,
    PlcRunningButton,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  isRunning = false

  isDataWatchOpen = false

  currentTab = 0

  tabs = [
    '运行状态', '随机读写'
  ]

  async getDataSource () {
    let response = await axios.post('/plc-workers/is-running', {
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
