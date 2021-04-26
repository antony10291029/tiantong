<template>
  <td>
    <span class="tag is-light" :class="`is-${display.type}`">
      {{display.text}}
    </span>
  </td>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";

export default defineComponent({
  name: "TheStatus",

  props: {
    value: {
      type: String,
    }
  },

  setup(props) {
    const display = computed(() => {
      switch (props.value) {
        case "started":
          return { text: "执行中", type: "primary" };
        case "closed":
          return { text: "已关闭", type: "warning" };
        case "finished":
          return { text: "已执行", type: "success" };
        default:
          return { text: "未执行", type: "light" };
      }
    });

    return {
      display
    };
  }
});
</script>
