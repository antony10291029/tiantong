<template>
  <div>
    <Navbar></Navbar>
    <router-view></router-view>
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
