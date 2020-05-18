<template>
  <AsyncLoader
    :handler="getIsAutorun"
    style="padding: 1.25rem"
  >
    <div class="field">
      <p class="title is-5">
        自动运行
      </p>

      <p class="content">
        当系统启动时，自动运行所有设备。
      </p>

      <div class="field">
        <div class="control">
          <AsyncButton
            v-if="!isAutorun"
            :handler="() => setAutorun(true)"
            class="button is-info is-light is-small"
          >
            开启
          </AsyncButton>
          <AsyncButton
            v-else
            :handler="() => setAutorun(false)"
            class="button is-info is-light is-small"
          >
            关闭
          </AsyncButton>
        </div>
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'Autorun'
})
export default class extends Vue {
  isAutorun = false

  async getIsAutorun () {
    let response = await axios.post('/autorun/get')
    this.isAutorun = response.data
  }

  async setAutorun (value: boolean) {
    await axios.post('/autorun/set', { value })
    await this.getIsAutorun()
  }
}
</script>
