<template>
  <router-view></router-view>
</template>

<script>
import axios from '@/providers/axios'

export default {
  name: 'App',
  created () {
    axios.post("/").then(response => {
      console.log(response.data);
    })
  }
}
</script>

<style lang="sass" src="@/assets/sass/_all.sass">
