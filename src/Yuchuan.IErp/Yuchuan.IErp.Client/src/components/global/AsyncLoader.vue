<template>
  <Loader
    v-bind="$attrs"
    :is-show="isPending"
    v-slot="slots"
  >
    <slot v-if="!isPending" v-bind="slots"></slot>
  </Loader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Loader from './Loader.vue'

@Component({
  name: 'AsyncLoader',
  components: {
    Loader
  },
  extends: Loader
})
export default class extends Vue {
  @Prop({ required: true })
  handler!: () => Promise<void>

  isPending: boolean = true

  async created () {
    try {
      await this.handler()
    } finally {
      this.isPending = false
    }
  }
}
</script>
