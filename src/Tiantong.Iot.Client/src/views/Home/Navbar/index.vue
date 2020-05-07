<template>
  <nav class="navbar has-border-bottom">
    <div class="navbar-brand">
      <span class="navbar-item is-size-4 is-unselectable has-text-link is-hoverable">
        天瞳 IOT
      </span>
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

          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'Navbar'
})
export default class extends Vue {
  isBurgerActive = false
  isProfileActive = false
}
</script>
