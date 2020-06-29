<template>
  <div class="app">
    <nav class="app-nav navbar">
      <div class="navbar-brand">
        <a
          class="navbar-item is-size-4 is-unselectable has-text-link"
          style="margin-left: 0.5rem"
        >
          天瞳账户
        </a>
      </div>

      <div class="navbar-menu">

      </div>
    </nav>

    <div class="app-body">
      <aside class="menu-container">
        <div
          class="menu is-unselectable"
          style="min-width: 260px; max-width: 260px"
        >
          <ul class="menu-list">
            <li>
              <router-link to="/profile">
                <span class="icon">
                  <i class="iconfont icon-account"></i>
                </span>
                <span>个人资料</span>
              </router-link>
            </li>
            <li>
              <a @click="handleLogout">
                <span class="icon">
                  <i class="iconfont icon-logout"></i>
                </span>
                <span>退出登陆</span>
              </a>
            </li>
          </ul>
        </div>
      </aside>
      <router-view class="is-flex-auto"></router-view>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { token } from '@/providers/local-storage'

@Component({
  name: 'Home',
  beforeRouteEnter (to, from, next) {
    if (token.get()) {
      next()
    } else {
      next('/login')
    }
  }
})
export default class extends Vue {
  handleLogout () {
    this.$confirm({
      title: '退出登陆',
      content: '退出后需要再次登陆',
      handler: () => {
        this.$router.push('/login')
        token.clear()
      }
    })
  }
}
</script>

<style lang="sass">
.app
  width: 100vw
  height: 100vh
  display: flex
  flex-flow: column

.app-nav
  min-height: 3.75rem
  max-height: 3.75rem

.app-body
  flex: 2
  width: 100vw
  display: flex
  overflow: hidden
</style>
