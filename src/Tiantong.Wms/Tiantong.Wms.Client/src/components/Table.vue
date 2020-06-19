<template>
  <table class="table is-bordered is-fullwidth is-vcentered is-centered is-nowrap">
    <slot></slot>
  </table>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'

@Component({
  name: 'Table'
})
export default class extends Vue {

}
</script>
