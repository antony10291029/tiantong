<template>
  <div class="app">
    <Navbar class="app-nav"></Navbar>
    <router-view class="app-body"></router-view>
  </div>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator'
import Navbar from './Navbar/index.vue'

@Component({
  name: 'Home',
  components: {
    Navbar
  }
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
  min-height: 52px
  max-height: 52px

.app-body
  flex: 2
  width: 100vw
  height: calc(100vh - 52px)
</style>
