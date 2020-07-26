<template>
  <router-view></router-view>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'App'
})
export default class extends Vue {
  created () {

  }
}
</script>

<style lang="sass" src="@zhanglan1315/vue-bulma/src/share/sass/_all.sass">
