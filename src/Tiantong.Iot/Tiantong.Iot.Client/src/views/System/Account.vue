<template>
  <AsyncLoader
    v-if="isLogged"
    :handler="getAccountInformation"
    style="padding: 1.25rem;"
  >
    <p class="title is-5 is-flex is-vcentered">
      <span style="margin-right: 0.5rem">账户状态</span>
      <span class="tag is-success is-light is-small">
        已登陆
      </span>
    </p>

    <p>
      <span>用户名：</span>
      <span>{{user.name}}</span>
    </p>

    <div style="height: 1.25rem"></div>

    <p>
      <span>邮箱：</span>
      <span>{{user.email}}</span>
    <p>

    </p>

    <div style="height: 1.25rem"></div>

    <div>
      <a
        @click="handleLogout"
        class="button is-info is-small is-light"
      >
        退出登陆
      </a>
    </div>
  </AsyncLoader>
  <section
    v-else
    style="padding: 1.25rem"
  >
    <p class="title is-5 is-flex is-vcentered">
      <span style="margin-right: 0.5rem">账户状态</span>
      <span class="tag is-success is-light is-small">
        未登录
      </span>
    </p>

    <p>登陆后能够使用更多安全功能。</p>

    <div style="height: 1.25rem"></div>

    <a
      @click="handleLogin"
      class="button is-info is-light is-small"
    >
      前往登陆
    </a>

  </section>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import account, { login } from '@/providers/context/account'
import { account as store } from '@/providers/local-storage'

@Component({
  name: 'Account'
})
export default class extends Vue {
  @Prop({ required: true })
  isLogged!: boolean

  user: any = {}

  async getAccountInformation () {
    const response = await account.post('/person/data')

    this.user = response.data
  }

  handleLogin () {
    login()
  }

  handleLogout () {
    this.$confirm({
      title: '退出登陆',
      content: '退出后需要重新登陆',
      handler: () => {
        store.clear()
        this.$emit('logout')
      }
    })
  }
}
</script>
