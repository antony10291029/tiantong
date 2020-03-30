<template>
  <td class="has-input">
    <input
      :value="value"
      @change="$emit('input', $event.target.value)"
      type="text" class="input"
    >
  </td>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'EditableCell',
  model: {
    prop: 'value',
    event: 'input'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  value: any
}
</script>
