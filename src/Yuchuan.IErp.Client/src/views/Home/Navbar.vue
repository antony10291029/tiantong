<template>
  <nav class="navbar">
    <div class="navbar-brand">
      <a class="navbar-item is-size-4 is-unselectable has-text-link">
        天瞳 ERP
      </a>
    </div>

    <div class="navbar-menu">
      <router-link
        class="navbar-item is-flex is-centered"
        to="/chanjet-hkj"
      >
        <span
          class="icon"
          style="margin-right: 0.25rem"
        >
          <i class="iconfont icon-charts"></i>
        </span>
        <span>财务报表</span>
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
