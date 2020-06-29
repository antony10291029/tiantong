<template>
  <LoginBackground>
    <div class="title is-4 has-text-centered">
      账号登陆
    </div>

    <div class="field">
      <label class="label">邮箱</label>
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
      <div class="is-flex">
        <p class="label">密码</p>
        <span class="is-flex-auto"></span>
        <router-link
          tabindex="-1"
          :to="{ path: `/reset-password/email`, query: $route.query }"
        >
          忘记密码
        </router-link>
      </div>
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
      <AsyncButton
        :handler="handleSubmit"
        v-class:is-loading="isPending"
        class="button is-vcentered is-info is-outlined"
      >
        登录
      </AsyncButton>
    </div>
    <div style="height: 1rem"></div>
    <div class="has-text-centered">
      <router-link
        :to="{ path: '/register', query: $route.query }"
        tabindex="-1"
      >
        没有账号？点击注册！
      </router-link>
    </div>
  </LoginBackground>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import LoginBackground from '@/components/LoginBackground/index.vue'
import account from '@/providers/contexts/account'
import router from '@/providers/vue-router'

@Component({
  name: 'Login',
  components: {
    LoginBackground
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  redirect!: string

  params = {
    email: '',
    password: '',
  }

  isPending = false

  async handleSubmit () {
    try {
      this.isPending = true
      await account.loginByEmail(this.params, this.redirect)
    } finally {
      this.isPending = false
    }
  }
}
</script>
