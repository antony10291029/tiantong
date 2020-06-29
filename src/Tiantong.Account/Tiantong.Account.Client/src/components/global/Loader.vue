<template>
  <component :is="tag">
    <VueLoading
      v-if="isLoading"
      type="spin"
      :style="style"
    />
    <slot v-else :isLoading="isLoading"></slot>
  </component>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { VueLoading } from 'vue-loading-template'

@Component({
  name: 'Loader',
  components: {
    VueLoading
  }
})
export default class extends Vue {
  @Prop({ default: 'div' })
  tag!: string

  @Prop({ default: '#118fe4' })
  color!: string

  @Prop({ default: 300 })
  delay!: number | string

  @Prop({ default: '0' })
  marginTop!: string

  @Prop({ default: '4rem' })
  size!: string

  @Prop({ default: false })
  isShow!: boolean

  isDelayEnd: boolean = false

  get style () {
    return {
      'fill': this.color,
      'width': this.size,
      'height': this.size,
      'top': '10px'
    }
  }

  get isLoading () {
    return this.isShow && this.isDelayEnd
  }

  handleDelay () {
    let delay = typeof this.delay === 'string'
      ? parseInt(this.delay)
      : this.delay

    if (delay !== 0 && delay !== NaN) {
      setTimeout(() => this.isDelayEnd = true, delay)
    }
  }

  created () {
    this.handleDelay()
  }
}
</script>
