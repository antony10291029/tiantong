<template>
  <LoginBackground>
    <div class="title is-4 has-text-centered">
      账号注册
    </div>

    <div class="field">
      <label class="label">邮箱</label>
      <div class="control has-icons-left">
        <input v-model="params.email" class="input" type="text">
        <span class="icon is-left">
          <i class="iconfont icon-email"></i>
        </span>
      </div>
    </div>

    <div class="field">
      <label class="label">验证码</label>
    </div>

    <div class="field has-addons">
      <div class="control has-icons-left is-expanded">
        <input
          v-model.lazy="params.verification_code"
          class="input" type="text"
        >
        <span class="icon is-left">
          <i class="iconfont icon-verification-code"></i>
        </span>
      </div>
      <div class="control">
        <VerificationCodeButton
          :address="params.email"
          @change="params.verification_key = $event"
        />
      </div>
    </div>

    <div class="field">
      <label class="label">密码</label>
      <div class="control has-icons-left">
        <input v-model="params.password" class="input" type="password">
        <span class="icon is-left">
          <i class="iconfont icon-password"></i>
        </span>
      </div>
    </div>

    <div class="field">
      <label class="label">姓名</label>
      <div class="control has-icons-left">
        <input
          class="input"
          v-model="params.name"
          @keypress.enter="handleSubmit"
        >
        <span class="icon is-left">
          <i class="iconfont icon-name"></i>
        </span>
      </div>
    </div>

    <div style="height: 1rem"></div>
    <div class="has-text-centered">
      <AsyncButton
        :handler="handleSubmit"
        class="button is-vcentered is-info is-outlined"
      >注册</AsyncButton>
    </div>

    <div style="height: 1rem"></div>
    <div class="has-text-centered">
      <router-link :to="{ path: '/login', query: $route.query }">
        已有账号，直接登录！
      </router-link>
    </div>
  </LoginBackground>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import LoginBackground from '@/components/LoginBackground/index.vue'
import account from '@/providers/contexts/account'
import VerificationCodeButton from '@/components/VerificationCodeButton/index.vue'

@Component({
  name: 'Register',
  components: {
    LoginBackground,
    VerificationCodeButton
  },
})
export default class extends Vue {
  @Prop({ default: '' })
  redirect!: string

  params = {
    email: '',
    password: '',
    name: '',
    verification_key: '',
    verification_code: '',
  }

  isPending = false

  async handleSubmit () {
    await account.registerByEmail(this.params)
    await account.loginByEmail(this.params, '')
  }
}
</script>
