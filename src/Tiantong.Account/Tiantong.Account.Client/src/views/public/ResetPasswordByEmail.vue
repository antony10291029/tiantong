<template>
  <LoginBackground>
    <div class="title is-4 has-text-centered">
      重置密码
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
      <label class="label">新密码</label>
      <div class="control has-icons-left">
        <input type="password" class="input" v-model="params.password">
        <span class="icon is-left">
          <i class="iconfont icon-password"></i>
        </span>
      </div>
    </div>

    <div style="height: 1rem"></div>

    <div class="has-text-centered">
      <a
        @click="handleSubmit"
        v-class:is-loading="isPending"
        class="button is-vcentered is-info is-outlined"
      >提交</a>
    </div>
  </LoginBackground>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import LoginBackground from '@/components/LoginBackground/index.vue'
import VerificationCodeButton from '@/components/VerificationCodeButton/index.vue'
import account from '@/providers/contexts/account'

@Component({
  name: 'ResetPasswordByEmail',
  components: {
    LoginBackground,
    VerificationCodeButton
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  redirect!: string

  params = {
    email: '',
    password: '',
    verification_key: '',
    verification_code: ''
  }

  isPending = false

  async handleSubmit () {
    await account.resetPasswordByEmail(this.params)
    this.$router.push({
      path: '/login',
      query: this.$route.query
    })
  }
}
</script>
