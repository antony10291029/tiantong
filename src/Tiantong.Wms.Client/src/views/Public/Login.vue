<template>
  <div style="height: 100vh; width: 100vw; overflow-y: hidden">
    <div class="columns is-vcentered is-gapless">
      <div
        class="login column is-3"
        style="min-width: 360px;"
      >
        <section class="section">
          <div class="title is-4 has-text-centered">
            WMS 登陆
          </div>

          <div class="field">
            <label class="label">电子邮箱</label>
            <div class="control has-icons-left">
              <input
                v-model.lazy="params.email"
                class="input" type="text"
              >
              <span class="icon is-left">
                <i class="iconfont icon-email"></i>
              </span>
            </div>
          </div>

          <div class="field">
            <label class="label">密码</label>
            <div class="control has-icons-left">
              <input
                v-model="params.password"
                @keypress.enter="handleSubmit"
                class="input" type="password"
              >
              <span class="icon is-left">
                <i class="iconfont icon-password"></i>
              </span>
            </div>
          </div>

          <div style="height: 0.5rem"></div>

          <div class="has-text-centered">
            <a
              v-loading="isPending"
              @click="handleSubmit"
              class="button is-vcentered is-info is-outlined"
            >登录</a>
          </div>
          <div style="height: 1rem"></div>
          <div class="has-text-centered">
            <router-link to="/register">
              没有账户？点击注册！
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
import router from '@/providers/router'
import { notify } from '@/providers/notify'
import LoginBackground from './common/LoginBackground.vue'

export async function handleLogin (params) {
  const response = await axios.post('/auth/email', params)
  token.set(response.data.token)
  router.push('/home')
}

export default {
  name: 'Login',
  components: {
    LoginBackground,
  },
  data: () => ({
    params: {
      email: '',
      password: '',
    },
    isPending: false
  }),
  methods: {
    async handleSubmit () {
      try {
        await handleLogin(this.params)
        notify.info('登陆成功')
      } catch (error) {
        notify.danger('登录失败，请重试')
        throw error
      }
    }
  },
}
</script>
