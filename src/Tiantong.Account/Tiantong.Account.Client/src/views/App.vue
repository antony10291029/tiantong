<template>
  <router-view></router-view>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'App'
})
export default class extends Vue {

}
</script>

<style lang="sass" src="@/assets/sass/_all.sass">
