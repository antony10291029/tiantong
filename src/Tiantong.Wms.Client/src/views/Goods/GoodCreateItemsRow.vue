<template>
  <tr>
    <slot></slot>
    <td>
      <a
        v-show="index !== 0"
        class="icon has-text-danger"
        @click="$emit('remove', index)"
      >
        <i class="iconfont icon-close"></i>
      </a>
    </td>
  </tr>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'GoodCreateItemsRow'
})
export default class extends Vue {
  @Prop({ required: true})
  index!: number
}
</script>
