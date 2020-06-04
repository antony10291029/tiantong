<template>
  <nav class="navbar has-border-bottom">
    <div class="navbar-brand">
      <a class="navbar-item is-size-4 is-unselectable has-text-link">
        天瞳 ERP
      </a>
    </div>

    <div
      class="navbar-menu"
      style="margin-left: 1rem"
    >
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
        <span>好会计</span>
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
