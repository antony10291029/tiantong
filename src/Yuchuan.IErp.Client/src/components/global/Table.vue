<template>
  <table>
    <slot name="head"></slot>
    <slot name="body">
      <tbody>
        <tr>
          <td
            :colspan="colspan"
            class="is-centered"
          >
            暂无数据
          </td>
        </tr>
      </tbody>
    </slot>
    <slot name="foot"></slot>
  </table>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Table'
})
export default class extends Vue {
  @Prop({ default: 0 })
  colspan!: string | number
}
</script>
