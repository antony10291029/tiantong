<template>
  <div class="app">
    <div class="app-nav navbar">
      <div class="navbar-brand">
        <a class="navbar-item is-size-4 is-unselectable has-text-link">
          玉串 WCS
        </a>
      </div>
    </div>

    <div class="app-body">
      <aside
        class="menu is-unselectable"
        style="min-width: 260px; max-width: 260px"
      >
        <ul class="menu-list">
          <!-- <li>
            <router-link to="/" exact-active-class="is-active" active-class="false">
              <span class="icon">
                <i class="iconfont icon-dashboard"></i>
              </span>
              <span>控制面板</span>
            </router-link>
          </li> -->
          <li>
            <router-link to="/" exact-active-class="is-active" active-class="false">
              <span class="icon">
                <i class="iconfont icon-lifter"></i>
              </span>
              <span>提升机系统</span>
            </router-link>
          </li>
          <li>
            <router-link to="/auto-doors">
              <span class="icon">
                <i class="iconfont icon-lifter"></i>
              </span>
              <span>自动门系统</span>
            </router-link>
          </li>
          <li>
            <router-link to="/lifter">
              <span class="icon">
                <i class="iconfont icon-lifter"></i>
              </span>
              <span>货梯改造系统</span>
            </router-link>
          </li>
        </ul>
      </aside>
      <router-view class="is-flex-auto" />
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Home'
})
export default class extends Vue {

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
