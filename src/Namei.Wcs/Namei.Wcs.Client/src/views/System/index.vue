<template>
  <AsyncLoader
    :handler="initialize"
    style="padding: 1.25rem"
  >
    <div class="box">
      <p class="title is-5">
        系统配置
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <p class="label" style="width: 180px">
          执行货梯指令
        </p>

        <Switcher
          :value="enableLifterCommands"
          @input="changeEnableLifterCommands"
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
          :value="enableDoorsCommands"
          @input="changeEnableDoorsCommands"
        />
      </div>

      <hr>

    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'System'
})
export default class extends Vue {
  enableLifterCommands = false

  enableDoorsCommands = false

  async getLifterSettings () {
    const response = await domain.post('/test/enable-lifter-commands')
    this.enableLifterCommands = response.data.value
  }

  async getDoorsSettings () {
    const response = await domain.post('/test/enable-doors-commands')
    this.enableDoorsCommands = response.data.value
  }

  async changeEnableLifterCommands (value: boolean) {
    await domain.post('/test/set-enable-lifter-commands', { value })
    await this.initialize()
  }

  async changeEnableDoorsCommands (value: boolean) {
    await domain.post('/test/set-enable-doors-commands', { value })
    await this.initialize()
  }

  async initialize () {
    await Promise.all([
      this.getDoorsSettings(),
      this.getLifterSettings(),
    ])
  }
}
</script>
