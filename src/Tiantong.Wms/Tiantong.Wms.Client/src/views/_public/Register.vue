<template>
  <div style="height: 100vh; width: 100vw; overflow-y: hidden">
    <div class="columns is-vcentered is-gapless">
      <div
        class="login column is-3"
        style="min-width: 360px"
      >
        <section class="section">
          <div class="title is-4 has-text-centered">
            WMS 注册
          </div>

          <div class="field">
            <label class="label">电子邮箱</label>
            <div class="control has-icons-left">
              <input v-model.lazy="params.email" class="input" type="text">
              <span class="icon is-left">
                <i class="iconfont icon-email"></i>
              </span>
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
              @click="handleSubmit(params)"
            >注册</AsyncButton>
          </div>

          <div style="height: 1rem"></div>
          <div class="has-text-centered">
            <router-link to="/login">
              已有账户，直接登录！
            </router-link>
          </div>
        </section>
      </div>
      <login-background></login-background>
    </div>
  </div>
</template>

<script>
import axios from '@/providers/axios'
import token from '@/providers/token'
import { handleLogin } from './Login'
import LoginBackground from './common/LoginBackground/index.vue'
import AsyncButton from '@/components/AsyncButton.vue'

export default {
  name: 'Register',
  components: {
    LoginBackground,
    AsyncButton,
  },
  data: () => ({
    params: {
      email: '',
      password: '',
      name: '',
    },
    isPending: false
  }),
  methods: {
    async handleSubmit () {
      try {
        this.isPending = true
        await axios.post('/users/register', this.params)
        await handleLogin(this.params)
      } catch (error) {
        this.isPending = false
        throw error
      }
    }
  }
}
</script>
