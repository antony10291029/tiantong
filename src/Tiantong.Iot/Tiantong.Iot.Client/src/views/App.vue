<template>
  <router-view></router-view>
</template>

<script>
import axios from '@/providers/axios'

export default {
  name: 'App',
  async created () {
    let response = await axios.post("/")

    console.log(response.data)
  }
}
</script>

<style lang="sass" src="@/share/sass/_all.sass">
