<template>
  <div style="height: 100vh; width: 100vw; overflow-y: hidden">
    <div class="columns is-vcentered is-gapless">
      <div
        class="login column is-3"
        style="min-width: 360px;"
      >
        <section class="section">
          <div class="title is-4 has-text-centered">
            畅捷通登陆（好会计）
          </div>

          <div class="field">
            <label class="label">账户</label>
            <div class="control has-icons-left">
              <input
                v-model.lazy="params.account"
                class="input" type="text"
              >
              <span class="icon is-left">
                <i class="iconfont icon-account"></i>
              </span>
            </div>
          </div>

          <div class="field">
            <div class="is-flex">
              <p class="label">密码</p>
              <span class="is-flex-auto"></span>
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
            <button
              @click="handleSubmit"
              v-class:is-loading="isPending"
              class="button is-vcentered is-info is-outlined"
            >
              登录
            </button>
          </div>
        </section>
      </div>
      <login-background></login-background>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import LoginBackground from '@/components/LoginBackground.vue'
import chanjet from '@/providers/contexts/chanjet'
import localStorage from '@/providers/local-storage'
import notify from '@/providers/notify'
import cookie from 'js-cookie'

@Component({
  name: 'ChanjetLogin',
  components: {
    LoginBackground
  }
})

export default class extends Vue {
  @Prop({ default: '/charts' })
  redirect!: string

  params = {
    account: '',
    password: '',
  }

  isPending = false

  async login () {
    await chanjet.logout()

    const code = (await chanjet.getChanjetCode())
    const userData = await chanjet.getUserData(code, this.params.account, this.params.password)
    const vmToken = (await chanjet.getVmCode())
    const result = await chanjet.loginVm(vmToken)
    const { orgCode, bookCode } = await chanjet.checkVmStatus(userData.orgId)
    const tokenCode = await chanjet.getHkjCode()
    const token = await chanjet.getToken(tokenCode, orgCode, bookCode)

    localStorage.chanjet.token.set(token)
    localStorage.chanjet.orgId.set(userData.orgId)
    localStorage.chanjet.orgCode.set(orgCode)
    localStorage.chanjet.bookCode.set(bookCode)

    this.$router.push(`/chanjet-hkj/${orgCode}/${bookCode}`)
  }

  async handleSubmit () {
    try {
      this.isPending = true
      await this.login()
    } finally {
      this.isPending = false
    }
  }
}
</script>
