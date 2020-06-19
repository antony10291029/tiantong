<template>
  <input
    :value="value"
    v-on="listeners()"
  >
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Input',
  model: {
    prop: 'value',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  value: any

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
    let value: string = event.target.value

    if (value === '') {
      value = this.default
    }

    this.$emit(key, value)
  }
}
</script>
