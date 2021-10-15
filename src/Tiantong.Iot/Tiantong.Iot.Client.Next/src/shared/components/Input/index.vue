<template>
  <input
    type="text"
    class="
      px-3 py-1.5 w-full
      relative rounded
      outline-none
      border border-dark-700 focus:border-link-500
      hover:border-dark-600
      ring-1 ring-transparent focus:ring-opacity-30 focus:ring-link-500
      bg-transparent focus:bg-dark-800
    "
    :value="value"
    v-on:[event]="handleUpdate"
    ref="el"
  >
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted } from "vue";

type Event = "change" | "input" | "blur";

export default defineComponent({
  props: {
    value: {
      type: String,
      default: ""
    },

    event: {
      type: String as PropType<Event>,
      default: "change"
    }
  },

  emits: [
    "mounted",
    "update:value",
  ],

  setup(props, { emit }) {
    const el = ref<HTMLInputElement>();
    const handleUpdate = (event: any) => {
      emit("update:value", event.target.value.trim());
    };

    onMounted(() => emit("mounted", el.value));

    return {
      el,
      handleUpdate
    };
  }
});
</script>
