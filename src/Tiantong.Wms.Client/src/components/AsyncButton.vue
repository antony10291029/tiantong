<template>
  <component @click="handleClick" v-loading="" :is="tag">
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

  isPending: boolean = false

  async handleClick () {
    try {
      this.isPending = true
      await this.handler()
    } finally {
      this.isPending = false
    }
  }
}
</script>
