<template>
  <input
    type="text"
    class="
      input
      px-3 py-1.5 w-full
      relative rounded
      outline-none bg-transparent
      border border-dark-700
      ring-2 ring-transparent
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
    },

    readonly: {
      type: Boolean,
      default: false
    },
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
