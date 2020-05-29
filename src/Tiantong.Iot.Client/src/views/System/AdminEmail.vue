<template>
  <AsyncLoader
    :handler="getAdminEmail"
    style="padding: 1.25rem"
  >
    <p class="title is-5">
      管理员邮箱
    </p>

    <template v-if="currentEmail === ''">
      <p class="content">
        管理员邮箱未绑定，请尽快设置。
      </p>
      <div class="field">
        <label class="label">邮箱</label>
      </div>
      <div class="field has-addons" style="width: 360px">
        <div class="control is-expanded">
          <input
            v-model="email"
            type="text" class="input"
          >
        </div>
        <div class="control">
          <AsyncButton
            v-if="emailVerifyCodeInterval === 0"
            :handler="getEmailVerifyCode"
            class="button is-info is-light is-bordered"
            style="width: 120px"
          >
            发送验证码
          </AsyncButton>
          <a v-else
            disabled
            class="button is-static"
            style="width: 120px"
          >
            {{emailVerifyCodeInterval}}
          </a>
        </div>
      </div>

      <label class="label">验证码</label>

      <div class="field has-addons" style="width: 360px">
        <div class="control is-expanded">
          <input
            v-model="verifyCode"
            type="text" class="input"
          >
        </div>
        <div class="control">
          <AsyncButton
            v-if="emailVerifyCodeId !== 0 && verifyCode !== ''"
            :handler="setAdminEmail"
            style="width: 120px"
            class="button is-info is-light is-bordered"
          >
            确认绑定
          </AsyncButton>
          <a
            v-else
            style="width: 120px"
            class="button is-static"
          >
            确认绑定
          </a>
        </div>
      </div>
    </template>
    <template v-else>
      <p class="content">
        已绑定邮箱 {{currentEmail}}
      </p>

      <AsyncButton
        v-if="emailVerifyCodeId === 0"
        :handler="getEmailVerifyCode"
        class="button is-info is-light is-small"
      >
        解除绑定
      </AsyncButton>
      <template v-else>
        <div class="field" style="width: 320px">
          <label class="label">验证码</label>
          <div class="control">
            <input
              v-model="verifyCode"
              type="text" class="input"
            >
          </div>
        </div>

        <div class="field">
          <div class="control">
            <AsyncButton
              v-if="verifyCode !== ''"
              :handler="unsetAdminEmail"
              class="button is-info is-light is-small is-bordered"
            >
              解除绑定
            </AsyncButton>
            <a
              v-else
              class="button is-static is-small"
            >
              解除绑定
            </a>

            <a
              @click="emailVerifyCodeId = 0"
              style="margin-left: 0.5rem"
              class="button is-info is-light is-small"
            >
              返回
            </a>
          </div>
        </div>
      </template>

    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'AdminEmail'
})
export default class extends Vue {
  email = ''

  currentEmail = ''

  emailVerifyCodeId = 0

  emailVerifyCodeInterval = 0

  verifyCode = ''

  intervalId = 0

  timeoutId = 0

  setInterval () {
    this.emailVerifyCodeInterval = 60
    this.intervalId = setInterval(() => this.emailVerifyCodeInterval--, 1000)
    this.timeoutId = setTimeout(() => clearTimeout(this.intervalId), 60000)
  }

  clearInterval () {
    clearInterval(this.intervalId)
    clearTimeout(this.timeoutId)
    this.emailVerifyCodeInterval = 0
    this.emailVerifyCodeId = 0
  }

  async getAdminEmail () {
    let response = await axios.post('/admin-email/get')

    this.currentEmail = this.email = response.data
    this.verifyCode = ''
    this.clearInterval()
  }

  async getEmailVerifyCode () {
    let response = await axios.post('/email-verify-code/create', {
      email: this.email
    })

    this.emailVerifyCodeId = response.data.id
    this.setInterval()
  }

  async setAdminEmail () {
    await axios.post('/admin-email/set', {
      email_verify_code_id: this.emailVerifyCodeId,
      email: this.email,
      verify_code: this.verifyCode
    })
    await this.getAdminEmail()
  }

  async unsetAdminEmail () {
    await axios.post('/admin-email/unset', {
      email_verify_code_id: this.emailVerifyCodeId,
      email: this.email,
      verify_code: this.verifyCode
    })
    await this.getAdminEmail()
  }
}
</script>
