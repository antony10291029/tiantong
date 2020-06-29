<template>
  <textarea
    :rows="rows"
    v-bind="$attrs"
    class="textarea"
    v-on="listeners"
    v-text="value"
  ></textarea>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'TextArea',
  model: {
    prop: 'value',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: 4 })
  rows!: number

  @Prop({ required: true })
  value: any

  @Prop({ default: '' })
  default!: any

  get listeners () {
    let listeners: any = {}

    for (let key in this.$listeners) {
      let listener = this.$listeners[key]

      listeners[key] = Array.isArray(listener)
        ? listener.map(func => (event: any) => this.handleEvent(key, event))
        : (event: any) => this.handleEvent(key, event)
    }

    return listeners 
  }

  asyncRows () {
    var el = this.$el as any

    const rows = el.value.split(/\r*\n/).length
    if (rows > this.rows) {
      el.rows = rows
    }
  }

  handleEvent (key: string, event: any) {
    this.asyncRows()
    let value: string = event.target.value

    if (value === '') {
      value = this.default
    }

    this.$emit(key, value)
  }

  mounted () {
    this.asyncRows()
  }
}
</script>
