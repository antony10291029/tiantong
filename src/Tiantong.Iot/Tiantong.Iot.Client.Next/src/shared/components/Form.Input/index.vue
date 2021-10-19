<template>
  <input
    type="text"
    class="input"
    :value="value"
    v-on:[event]="handleUpdate"
    ref="el"
  >
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted } from "vue";
import "../Form/input.css";

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
