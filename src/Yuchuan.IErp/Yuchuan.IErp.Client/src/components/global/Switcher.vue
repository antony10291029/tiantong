<template>
  <div class="field">
    <input
      :checked="value"
      type="checkbox" class="switch is-info"
    >
    <label @click="$emit('input', !value)"></label>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Switcher'
})
export default class extends Vue {
  @Prop({ default: true })
  value!: boolean
}
</script>
