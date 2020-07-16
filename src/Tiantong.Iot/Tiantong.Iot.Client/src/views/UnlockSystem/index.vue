<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div
      class="card is-radius is-flex is-vcentered has-background-white-bis"
      style="width: 400px; height: 220px"
    >
      <div class="card-content is-flex-auto" style="padding: 4rem">
        <div class="field">
          <p class="title is-5">
            系统已锁定，请输入密码
          </p>
          <div class="field has-addons">
            <div class="control is-expanded">
              <input
                v-model="password"
                type="password" class="input"
                @keypress.enter="unlock"
              >
            </div>
            <div class="control">
              <AsyncButton
                :handler="unlock"
                class="button is-info"
              >
                <span class="icon">
                  <i class="iconfont icon-enter"></i>
                </span>
              </AsyncButton>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import systemLock from '@/providers/system-lock'

@Component({
  name: 'UnlockSystem'
})
export default class extends Vue {
  password = ''

  async unlock () {
    await systemLock.unlock(this.password)
    this.$router.push('/')
  }
}
</script>
