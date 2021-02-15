<template>
  <div>
    <slot v-if="!isPending"></slot>
    <div v-else></div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";

export default defineComponent({
  name: "AsyncLoader",

  props: {
    handler: {
      type: Function as PropType<() => void>,
    }
  },

  setup(props) {
    const isPending = ref(true);

    const handle = async () => {
      try {
        if (props.handler) {
          await props.handler();
        }
      } finally {
        isPending.value = false;
      }
    };

    handle();

    return {
      isPending
    };
  }
});
</script>
