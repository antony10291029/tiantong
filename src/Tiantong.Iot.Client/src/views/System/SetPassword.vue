<template>
  <AsyncLoader
    :handler="getHasPassword"
    style="padding: 1.25rem"
  >
    <p class="title is-5">
      系统密码
    </p>

    <p class="content">
      系统密码用于安全相关的操作，设置后请妥善保管。
    </p>

    <template v-if="!hasPassword">
      <p class="content">
        系统暂时无密码，请尽快设置。
      </p>

      <div class="field" style="width: 320px">
        <label class="label">密码</label>
        <div class="control">
          <input type="password" class="input" v-model="setParams.password">
        </div>
      </div>

      <div class="field" style="width: 320px">
        <label class="label">密码确认</label>
        <div class="control">
          <input
            type="password" class="input"
            @keypress.enter="handleSetPassword"
            v-model="setParams.password_confirmation"
          >
        </div>
      </div>

      <AsyncButton
        :handler="handleSetPassword"
        class="button is-info is-light is-small"
      >
        提交
      </AsyncButton>
    </template>
    <template v-else>
      <template v-if="!isResetPasswordShow && !isForgetPasswordShow">
        <a
          @click="isResetPasswordShow = true"
          style="margin-right: 0.5rem"
          class="button is-info is-light is-small"
        >
          修改密码
        </a>

        <a
          @click="isForgetPasswordShow = true"
          class="button is-info is-light is-small"
        >
          忘记密码
        </a>
      </template>

      <template v-if="isResetPasswordShow">
        <div class="field" style="width: 320px">
          <label class="label">
            旧密码
          </label>
          <div class="control">
            <input
              v-model="resetParams.old_password"
              type="password" class="input" style="width: 320px"
            >
          </div>
        </div>

        <div class="field" style="width: 320px">
          <label class="label">
            新密码
          </label>
          <div class="control">
            <input
              v-model="resetParams.password"
              type="password" class="input" style="width: 320px"
            >
          </div>
        </div>

        <div class="field" style="width: 320px">
          <label class="label">
            确认密码
          </label>
          <div class="control">
            <input
              type="password" class="input"
              @keypress.enter="handleResetPassword"
              v-model="resetParams.password_confirmation"
            >
          </div>
        </div>

        <AsyncButton
          :handler="handleResetPassword"
          class="button is-info is-light is-small"
          style="margin-right: 0.5rem"
        >
          确认修改
        </AsyncButton>

        <a
          @click="isResetPasswordShow = false"
          class="button is-info is-light is-small"
        >
          返回
        </a>
      </template>

      <template v-if="isForgetPasswordShow">
        <template v-if="emailVerifyCodeId === 0">
          <AsyncButton
            :handler="sendEmailCode"
            style="margin-right: 0.5rem"
            class="button is-info is-light is-small"
          >
            发送验证码至邮箱
          </AsyncButton>
          <a
            @click="isForgetPasswordShow = false"
            class="button is-info is-light is-small"
          >
            返回
          </a>
        </template>
        <template v-else>
          <div class="field" style="width: 320px">
            <label class="label">验证码</label>
            <div class="control">
              <input v-model="verifyCode" type="text" class="input">
            </div>
          </div>
          <div>
            <div class="control">
              <AsyncButton
                :handler="unsetAdminPassword"
                style="margin-right: 0.5rem"
                class="button is-info is-light is-small"
              >
                重置密码
              </AsyncBUtton>
              <a
                @click="emailVerifyCodeId = 0"
                style="margin-right: 0.5rem"
                class="button is-info is-light is-small"
              >
                返回
              </a>
            </div>
          </div>
        </template>
      </template>
    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'SetPassword'
})
export default class extends Vue {
  hasPassword = false

  isResetPasswordShow = false

  isForgetPasswordShow = false

  isResetPasswordByEmailShow = false

  verifyCode = ''

  emailVerifyCodeId = 0

  setParams = {
    password: '',
    password_confirmation: ''
  }

  resetParams = {
    old_password: '',
    password: '',
    password_confirmation: ''
  }

  async getHasPassword () {
    let response = await axios.post('/system-password/has')

    this.hasPassword = response.data
    this.emailVerifyCodeId = 0
    this.verifyCode = ''
    this.isResetPasswordShow = false
    this.isForgetPasswordShow = false
    this.isResetPasswordByEmailShow = false
  }

  async handleSetPassword () {
    await axios.post('/system-password/set', this.setParams)
    await this.getHasPassword()
  }

  async handleResetPassword () {
    await axios.post('/system-password/reset', this.resetParams)
    await this.getHasPassword()
  }

  async sendEmailCode () {
    let response = await axios.post('/system-password/send-email-code')
    this.emailVerifyCodeId = response.data.id
  }

  async unsetAdminPassword () {
    await axios.post('/system-password/unset', {
      email_verify_code_id: this.emailVerifyCodeId,
      verify_code: this.verifyCode
    })
    await this.getHasPassword()
  }
}
</script>
