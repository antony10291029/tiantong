<template>
  <td
    ref="td"
    v-if="!readonly"
    v-class:is-hoverable="hoverable"
    contenteditable
    v-bind="$attrs"
    v-text="value"
    @focus="handleFocus"
    @blur="handleChange"
    @keypress.enter="handleChange"
  />
  <td
    v-else
    v-text="value"
  />
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

  @Prop({ default: false })
  readonly!: boolean

  @Prop({ default: true })
  hoverable!: boolean

  isFocused: boolean = false

  getDefaultValue () {
    if (this.default !== '') {
      return this.default
    } else {
      switch (this.type) {
        case 'number': return 0
        default: return ''
      }
    }
  }

  getValue (event: any) {
    let value = event.target.innerText.trim()

    if (value === '') {
      value = this.getDefaultValue()
    }

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
