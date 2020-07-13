<template>
  <div
    v-class:is-active="isShow"
    class="modal"
  >
    <div class="modal-background"></div>

    <div class="modal-card" style="width: 400px">
      <div class="modal-card-head">
        <p class="modal-card-title">
          确认管理员密码
        </p>
      </div>

      <div class="modal-card-body">
        <div class="field">
          <p class="label" style="margin-bottom: 0.5rem">
            输入密码
          </p>
          <div class="control">
            <input
              class="input"
              type="password"
              v-model="password"
              @keypress.enter="confirm"
            >
          </div>
        </div>
      </div>

      <div class="modal-card-foot">
        <AsyncButton class="button is-success" :handler="confirm">
          确认
        </AsyncButton>
        <a
          class="button"
          @click="isShow = false"
        >
          取消
        </a>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'AdminComponent'
})
export default class extends Vue {
  isShow = false

  password = ''

  handler = (password: string) => {}

  open (handler: () => {}) {
    this.handler =  handler
    this.password = ''
    this.isShow = true
  }

  async confirm () {
    await this.handler(this.password)
    this.isShow = false
  }
}
</script>
