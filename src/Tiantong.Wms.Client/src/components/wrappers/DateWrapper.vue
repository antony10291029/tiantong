<template>
  <component v-if="!isMinValue" :is="tag">
    {{value && !isMinValue ? value.substr(0, 10) : $props.default}}
  </component>
  <component class="is-italic" v-else :is="tag">
    无
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
}
</script>
