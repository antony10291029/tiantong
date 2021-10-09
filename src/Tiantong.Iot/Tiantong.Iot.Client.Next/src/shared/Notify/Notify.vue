<template>
  <div
    class="
      shadow-lg rounded-lg whitespace-nowrap
      text-gray-300 mb-4 z-50 overflow-hidden
      flex items-center bg-dark-700
      w-full max-w-full sm:w-80 sm:min-w-max
    "
    @mouseover="handleMouseover"
    @mouseout="handleMouseout"
  >
    <div class="rounded-l-lg pl-5 py-4">
      <!-- Tailwind Purge   -->
      <!-- text-info-500    -->
      <!-- text-link-500    -->
      <!-- text-danger-500  -->
      <!-- text-success-500 -->
      <!-- text-warning-500 -->
      <i :class="`iconfont text-2xl icon-${type} text-${type}-500`" />
    </div>

    <span class="text-md ml-4 flex-auto pr-4">
      {{ content }}
    </span>

    <button
      class="
        hover:text-dark-100 hover:bg-dark-600
        px-5 py-5 rounded-r-lg
      "
      @click="$emit('close')"
    >
      <i class="iconfont icon-close" />
    </button>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted } from "vue";

export default defineComponent({
  props: {
    type: {
      type: String,
      required: true
    },

    content: {
      type: String,
      required: true
    },

    duration: {
      type: Number,
      required: true
    }
  },

  emits: [
    "close"
  ],

  setup(props, { emit }) {
    let timeoutId: any;
    let isHovered = false;

    const handleMouseover = () => isHovered = true;
    const handleMouseout = () => isHovered = false;
    const handleClose = () => {
      const intervalId = setInterval(() => {
        if (!isHovered) {
          clearTimeout(timeoutId);
          clearInterval(intervalId);
          emit("close");
        }
      }, 1000);
    };

    if (props.duration !== -1) {
      onMounted(() => timeoutId = setTimeout(handleClose, props.duration));
    }

    return {
      handleClose,
      handleMouseover,
      handleMouseout,
    };
  }
});
</script>
