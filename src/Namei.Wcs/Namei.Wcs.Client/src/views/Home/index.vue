<template>
  <div class="app">
    <div class="app-nav navbar">
      <div class="navbar-brand">
        <a
          @click="handleLogoClick"
          class="navbar-item is-size-4 is-unselectable has-text-link"
        >
          玉串 WCS
        </a>
      </div>
    </div>

    <div class="app-body">
      <aside
        v-if="isMenuShow"
        class="menu is-unselectable"
        style="min-width: 260px; max-width: 260px"
      >
        <ul class="menu-list">
          <li>
            <router-link to="/devices" active-class="false" exact-active-class="is-active">
              <span class="icon">
                <i class="iconfont icon-dashboard"></i>
              </span>
              <span>设备管理</span>
            </router-link>
          </li>
          <li>
            <router-link to="/data-screen">
              <span class="icon">
                <i class="iconfont icon-monitor"></i>
              </span>
              <span>数据分析</span>
            </router-link>
          </li>
          <li>
            <router-link to="/lifters">
              <span class="icon">
                <i class="iconfont icon-lifter"></i>
              </span>
              <span>提升机日志</span>
            </router-link>
          </li>
          <li>
            <router-link to="/doors">
              <span class="icon">
                <i class="iconfont icon-door"></i>
              </span>
              <span>自动门日志</span>
            </router-link>
          </li>
          <li>
            <router-link to="/devices/errors">
              <span class="icon">
                <i class="iconfont icon-warn"></i>
              </span>
              <span>故障信息</span>
            </router-link>
          </li>
          <li>
            <router-link to="/system">
              <span class="icon">
                <i class="iconfont icon-settings"></i>
              </span>
              <span>系统设置</span>
            </router-link>
          </li>
        </ul>
      </aside>
      <router-view class="is-flex-auto" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'Home',

  data: () => ({
    isMenuShow: true,
  }),

  methods: {
    handleLogoClick () {
      this.isMenuShow = !this.isMenuShow
    }
  }
})
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
