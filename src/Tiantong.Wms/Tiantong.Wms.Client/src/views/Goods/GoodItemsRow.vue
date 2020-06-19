<template>
  <tr class="is-unselectable">
    <td class="is-centered">{{index + 1}}</td>
    <slot></slot>
    <td>
      <a
        v-show="count !== 1"
        class="icon has-text-danger"
        @click="$emit('delete', index)"
      >
        <i class="iconfont icon-close"></i>
      </a>
    </td>
  </tr>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'GoodItemsRow'
})
export default class extends Vue {
  @Prop({ required: true})
  index!: number

  @Prop({ required: true })
  count!: number
}
</script>
