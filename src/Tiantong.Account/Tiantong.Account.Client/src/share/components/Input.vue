<template>
  <input
    :value="value"
    v-on="listeners()"
  >
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Input'
})
export default class extends Vue {
  @Prop({ required: true })
  value: any

  @Prop({ default: 'text' })
  type!: string

  @Prop({ default: '' })
  default!: any

  listeners () {
    let listeners: any = {}

    for (let key in this.$listeners) {
      let listener = this.$listeners[key]

      listeners[key] = Array.isArray(listener)
        ? listener.map(func => (event: any) => this.handleEvent(key, event))
        : (event: any) => this.handleEvent(key, event)
    }

    return listeners 
  }

  handleEvent (key: string, event: any) {
    let value: any = event.target.value

    if (this.type === 'int' || this.type == 'integer') {
      value = parseInt(value);
    }

    if (this.type === 'number' || this.type === 'float' || this.type === 'double') {
      value = parseFloat(value);
    }

    if (Number.isNaN(value)) {
      value = this.default === '' ? 0 : this.default
    }

    if (value === '') {
      value = this.default
    }

    console.log(value)

    this.$emit(key, value)
  }
}
</script>
