<template>
  <component
    :is="tag"
    :class="text === '无' && 'is-italic has-text-grey-light'"
  >
    {{text}}
  </component>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { DateTime } from '@/utils/common'

@Component({
  name: 'TimeWrapper'
})
export default class extends Vue {
  @Prop({ required: true })
  value!: string

  @Prop({ default: 'span' })
  tag!: string

  @Prop({ default: '无' })
  default!: string

  get isMinValue () {
    return this.value === DateTime.minValue
  }

  get text () {
    if (this.isMinValue) {
      return this.default
    } else {
      return this.value.substr(0, 10)
    }
  }
}
</script>
