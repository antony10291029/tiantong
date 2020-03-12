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
              <input v-model.lazy="params.password" class="input" type="password">
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
                v-model.lazy="params.name"
                @keypress.enter="handleSubmit"
              >
              <span class="icon is-left">
                <i class="iconfont icon-name"></i>
              </span>
            </div>
          </div>

          <div style="height: 1rem"></div>
          <div class="has-text-centered">
            <a
              v-loading="isPending"
              class="button is-vcentered is-info is-outlined"
              @click="handleSubmit(params)"
            >注册</a>
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
import LoginBackground from './common/LoginBackground.vue'

export default {
  name: 'Register',
  components: {
    LoginBackground,
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
      } catch (error) {
        this.$notify.danger('注册失败，请重试')
        this.isPending = false
        throw error
      }

      try {
        let response = await handleLogin(this.params)
        this.$notify.success('注册成功，欢迎使用')
      } catch (error) {
        this.$router.push('/login')
        this.$notify.success('注册成功，请登陆')
        throw error
      } finally {
        this.isPending = false
      }
    }
  }
}
</script>
