<template>
  <nav class="navbar has-border-bottom">
    <div class="navbar-brand">
      <span class="navbar-item is-size-4 is-unselectable has-text-link is-hoverable">
        天瞳WMS
      </span>

      <a
        class="navbar-burger burger"
        @click="isBurgerActive = !isBurgerActive"
        v-clickoutside="() => isBurgerActive = false"
      >
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>

    <div
      v-show="isBurgerActive"
      class="has-border-bottom" style="width: 100%; height: 0">
    </div>

    <div
      class="navbar-menu"
      v-active="isBurgerActive"
    >
      <div class="navbar-end is-unselectable">
        <div
          v-active="isProfileActive"
          v-clickoutside="() => isProfileActive = false"
          class="navbar-item has-dropdown"
          @click="isProfileActive = !isProfileActive"
        >
          <a class="navbar-item">
            <span
              class="icon has-text-link has-text-centered"
              style="width: 48px"
            >
              <i class="iconfont icon-profile is-size-4" />
            </span>
          </a>

          <div class="navbar-dropdown is-right">
            <router-link
              to="/setting"
              class="navbar-item"
              exact-active-class="is-active"
            >
              设置
            </router-link>
            <router-link
              to="/warehouses"
              class="navbar-item"
              exact-active-class="is-active"
            >
              全部仓库
            </router-link>
            <hr class="navbar-divider">
            <a
              @click="handleLogout"
              class="navbar-item"
            >
              <span style="margin-right: 0.125rem">
                退出登陆
              </span>
              <span class="icon is-right">
                <i class="iconfont icon-logout"></i>
              </span>
            </a>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import token from '@/providers/token'

export default {
  name: 'Navbar',
  data: () => ({
    isBurgerActive: false,
    isProfileActive: false,
  }),
  methods: {
    handleLogout () {
      token.clear()
      this.$router.push('/login')
    }
  }
}
</script>
