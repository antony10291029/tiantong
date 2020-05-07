<template>
  <router-view></router-view>
</template>

<script>
import axios from '@/providers/axios'

export default {
  name: 'App',
  created () {
    axios.get("/").then(response => {
      console.log(response.data);
    })
  }
}
</script>
