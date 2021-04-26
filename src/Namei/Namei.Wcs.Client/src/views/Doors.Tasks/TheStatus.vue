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
        case "requested":
          return { text: "等待中", type: "info" };
        case "entered":
          return { text: "通过中", type: "primary" };
        case "left":
          return { text: "已通过", type: "success" };
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
