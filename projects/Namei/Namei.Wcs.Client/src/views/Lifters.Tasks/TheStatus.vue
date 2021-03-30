<template>
  <td>
    <span class="tag is-light" :class="`is-${display.type}`">
      {{display.text}}
    </span>
  </td>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";

export default defineComponent({
  name: "TheStatus",

  props: {
    value: {
      type: String
    }
  },

  setup(props) {
    const display = computed(() => {
      switch (props.value) {
        case "imported":
          return { text: "已放货", type: "info" };
        case "exported":
          return { text: "取货中", type: "primary" };
        case "taken":
          return { text: "已取货", type: "success" };
        case "closed":
          return { text: "已关闭", type: "warning" };
        default:
          return { text: `未知状态 (${props.value})`, type: "danger" };
      }
    });

    return {
      display
    };
  }
});
</script>
