<template>
  <router-view></router-view>
</template>

<script>
export default {
  name: 'App',
  created () {
    console.log(document.cookie)
  }
}
</script>

<style lang="sass" src="@/assets/sass/_all.sass">
