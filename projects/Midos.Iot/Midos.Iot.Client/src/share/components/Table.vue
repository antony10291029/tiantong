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
import { defineComponent } from "vue";

export default defineComponent({
  name: "Table",

  props: {
    colspan: {
      type: [String, Number],
      default: 0
    }
  }
});
</script>
