<template>
  <router-view></router-view>
</template>

<script>
import axios from '@/providers/axios'
import token from '@/providers/token'

export default {
  name: 'App',
  created () {
    axios.post("/").then(response => {
      console.log(response.data.message)
    })

    token.initialize()
  }
}
</script>

<style lang="sass" src="@/assets/sass/_all.sass">
