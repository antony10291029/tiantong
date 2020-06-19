<template>
  <nav class="navbar has-border-bottom">
    <div class="navbar-brand">
      <a class="navbar-item is-size-4 is-unselectable has-text-link">
        天瞳 IOT
      </a>
    </div>

    <div
      class="navbar-menu"
      style="margin-left: 1rem"
    >
      <router-link
        class="navbar-item is-flex is-centered"
        to="/plcs"
      >
        <span
          class="icon"
          styl="margin-right: 0.25rem"
        >
          <i class="iconfont icon-server"></i>
        </span>
        <span>PLC 服务</span>
      </router-link>

      <router-link
        class="navbar-item is-flex is-centered"
        to="/system"
      >
        <span class="icon" style="margin-right; 0.25rem">
          <i class="iconfont icon-settings"></i>
        </span>
        <span>系统维护</span>
      </router-link>

    </div>
  </nav>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'Navbar'
})
export default class extends Vue {

}
</script>
