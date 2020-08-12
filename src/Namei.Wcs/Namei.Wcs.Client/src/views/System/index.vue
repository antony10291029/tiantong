<template>
  <AsyncLoader
    :handler="getSystemParams"
    style="padding: 1.25rem"
  >
    <div class="box">
      <p class="title is-5">
        系统配置
      </p>

      <hr>

      <ClearLogs />

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行货梯指令
        </p>

        <Switcher
          v-model="settings.enableLifterCommands"
          @input="setSystemParams"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行提升机指令
        </p>

        <Switcher
          @input="setSystemParams"
          v-model="settings.enableHoisterCommands"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行自动门指令
        </p>

        <Switcher
          @input="setSystemParams"
          v-model="settings.enableDoorsCommands"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行 WMS 指令
        </p>

        <Switcher
          v-model="settings.enableWmsCommands"
          @input="setSystemParams"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行 RCS 指令
        </p>

        <Switcher
          v-model="settings.enableRcsCommands"
          @input="setSystemParams"
        />
      </div>

      <hr>

    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'
import ClearLogs from './ClearLogs.vue'

@Component({
  name: 'System',
  components: {
    ClearLogs
  }
})
export default class extends Vue {
  settings = {
    enableDoorsCommands: false,
    enableLifterCommands: false,
    enableHoisterCommands: false,
    enableWmsCommands: false,
    enableRcsCommands: false,
  }

  async getSystemParams () {
    const response = await domain.post('/test/system-settings')

    this.settings = response.data
  }

  async setSystemParams () {
    await domain.post('/test/system-settings/set', this.settings)
    await this.getSystemParams()
  }
}
</script>
