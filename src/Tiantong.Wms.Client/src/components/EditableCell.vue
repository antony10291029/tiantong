<template>
  <td
    ref="td"
    contenteditable
    v-bind="$attrs"
    v-text="value"
    @focus="handleFocus"
    @blur="handleChange"
    @keypress.enter="handleChange"
  ></td>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'

@Component({
  name: 'EditableCell',
  model: {
    prop: 'value',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  value!: any

  @Prop({ default: 'text' })
  type!: string

  @Prop({ default: '' })
  default!: any

  isFocused: boolean = false

  getValue (event: any) {
    let value = event.target.innerText.trim()

    if (this.type === 'number') {
      value = parseFloat(value)

      if (Number.isNaN(value)) {
        value = 0
      }
    }

    return event.target.innerText = value
  }

  handleFocus (event: any) {
    this.$emit('focus', event)
    this.isFocused = true
    const range = document.createRange() as any
    const selection = window.getSelection() as any
    range.selectNodeContents(event.target)
    selection.removeAllRanges()
    selection.addRange(range)
  }

  handleChange (event: any) {
    this.$emit('change', this.getValue(event))
  }

}
</script>
