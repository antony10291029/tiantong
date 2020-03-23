<template>
  <table class="table is-bordered is-fullwidth is-vcentered is-centered is-nowrap is-hoverable">
    <slot></slot>
  </table>
</template>

<script>
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Table'
})
export default class extends Vue {

}
</script>
