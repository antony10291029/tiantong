<template>
  <AsyncLoader
    :handler="getIsLocked"
    style="padding: 1.25rem"
  >
    <div class="field">
      <p class="title is-5">
        系统锁定
      </p>

      <p class="content">
        生产环境请将系统锁定开启，防止设备信息被修改。
      </p>

      <div class="field">
        <div class="control">
          <AsyncButton
            v-if="!isLocked"
            :handler="lockSystem"
            class="button is-info is-light is-small"
          >
            开启
          </AsyncButton>
          <AsyncButton
            v-else
            :handler="unlockSystem"
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
  name: 'SystemLock'
})
export default class extends Vue {
  isLocked = false

  async getIsLocked () {
    let response = await axios.post('/system-lock/get')

    this.isLocked = response.data
  }

  lockSystem () {
    this.$confirmAdminPassword(async password => {
      await axios.post('/system-lock/lock', { password })
      await this.getIsLocked()
      this.$router.push('/unlock-system')
    })
  }

  unlockSystem () {
    this.$confirmAdminPassword(async password => {
      await axios.post('/system-lock/unlock', { password })
      await this.getIsLocked()
    })
  }
}
</script>
