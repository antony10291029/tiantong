<template>
  <span class="tag">
    {{text}}
  </span>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";

export default defineComponent({
  props: {
    value: {
      type: String,
      required: true
    }
  },

  setup(props) {
    const text = computed(() => {
      const [date, time] = props.value.split("T");

      return `${date} ${time.split(".")[0]}`;
    });

    return {
      text
    };
  }
});
</script>
