<template>
  <div>
    <slot :value="value"></slot>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  
})
export default class extends Vue {
  value = false
}
</script>
