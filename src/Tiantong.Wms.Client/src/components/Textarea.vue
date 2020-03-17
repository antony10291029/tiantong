<template>
  <textarea
    :rows="rows"
    v-bind="$attrs"
    class="textarea"
    @input="handleInput"
  ></textarea>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'TextArea'
})
export default class extends Vue {
  @Prop({ default: 4 })
  rows!: number

  asyncRows () {
    var el = this.$el as any

    const rows = el.value.split(/\r*\n/).length
    if (rows > this.rows) {
      el.rows = rows
    }
  }

  handleInput (event: any) {
    this.asyncRows()
    this.$emit('input', event.target.value)
  }
  mounted () {
    this.asyncRows()
  }
}
</script>
