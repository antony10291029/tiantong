<template>
  <div class="is-flex is-centered is-vcentered" style="width: 100vw; height: 100vh">
    <div class="box is-bordered" style="width: 400px">
      <p class="title is-size-4">密码重置</p>

      <div class="field">
        <p class="label">邮箱</p>
      </div>

      <div class="field has-addons">
        <div class="control is-expanded">
          <input
            v-model="email"
            type="text" class="input"
          >
        </div>
        <div class="control">
          <AsyncButton
            class="button is-info"
            :handler="handleSendEmail"
          >发送</AsyncButton>
        </div>
      </div>

      <div class="field">
        <p class="label">验证码</p>
        <div class="control">
          <input
            v-model="code"
            type="text" class="input"
          >
        </div>
      </div>

      <div class="field">
        <p class="label">新密码</p>
        <input
          v-model="password"
          type="password" class="input"
        >
      </div>

      <div class="field is-flex is-vcentered">
        <a @click="$router.go(-1)">返回</a>
        <div class="is-flex-auto"></div>
        <AsyncButton
          class="button is-info"
          :disabled="!isChanged"
          :handler="handlePasswordSubmit"
        >确认修改
        </AsyncButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'

@Component({
  name: 'PasswordReset',
  components: {
    AsyncButton
  }
})
export default class extends Vue {
  email: string = ''

  code: string = ''

  password: string = ''

  passwordResetId: number = 0

  get isChanged () {
    return this.code !== '' &&
      this.email !== '' &&
      this.password !== '' &&
      this.passwordResetId !== 0
  }

  async handleSendEmail () {
    let response = await axios.post('/password/reset/email/create', {
      email: this.email
    })

    this.passwordResetId = response.data.id
  }

  async handlePasswordSubmit () {
    await axios.post('/password/reset/email/submit', {
      id: this.passwordResetId,
      code: this.code,
      password: this.password
    })

    this.$router.push('/login')
  }
}
</script>
