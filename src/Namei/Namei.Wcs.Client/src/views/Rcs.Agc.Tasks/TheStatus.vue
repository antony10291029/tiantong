<template>
  <td>
    <span class="tag is-light" :class="`is-${data.type}`">
      {{data.text}}
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
      required: true
    }
  },

  setup(props) {
    const data = computed(() => {
      switch (props.value) {
        case "created": return { text: "已创建", type: "info" };
        case "started": return { text: "执行中", type: "primary" };
        case "finished": return { text: "已完成", type: "success" };
        case "closed": return { text: "已关闭", type: "warning" };
        default: return { text: `未知(${props.value})`, type: "" };
      }
    });

    return {
      data
    };
  },
});
</script>
