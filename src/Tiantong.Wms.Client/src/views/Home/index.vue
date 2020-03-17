<template>
  <div class="app">
    <Navbar></Navbar>
    <router-view class="app-body"></router-view>
  </div>
</template>

<script>
import Navbar from './Navbar/index.vue'

export default {
  name: 'Home',
  components: {
    Navbar
  }
}
</script>

<style lang="sass">
.app
  width: 100vw
  height: 100vh
  display: flex
  flex-flow: column

.app-body
  flex: 2
  width: 100vw
</style>
