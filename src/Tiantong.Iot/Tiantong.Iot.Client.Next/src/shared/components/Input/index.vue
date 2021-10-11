<template>
  <input
    type="text"
    class="
      px-3 py-1.5 w-full
      relative rounded
      outline-none
      border border-dark-700 focus:border-link-500
      ring-2 ring-transparent focus:ring-opacity-30 focus:ring-link-500
      bg-dark-900 hover:bg-dark-800 focus:bg-dark-700
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
