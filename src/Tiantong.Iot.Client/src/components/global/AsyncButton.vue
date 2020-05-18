<template>
  <component
    :class="!isPending || 'is-loading'"
    @click="handleClick"
    :is="tag"
    :disabled="disabled"
  >
    <slot></slot>
  </component>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'AsyncButton'
})
export default class extends Vue {
  @Prop({ default: 'a' })
  tag!: string

  @Prop({ required: true })
  handler!: () => Promise<void>

  @Prop({ default: false })
  disabled!: boolean

  isPending: boolean = false

  async handleClick () {
    if (this.disabled) return

    try {
      this.isPending = true
      await this.handler()
    } finally {
      this.isPending = false
    }
  }
}
</script>
