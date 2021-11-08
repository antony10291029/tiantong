<template>
  <div
    :class="`
      bg-dark-800 h-full
      flex-shrink-0 text-gray-400
      overflow-y-auto overflow-x-hidden
    `"
    style="width: 60px"
  >
    <div class="flex items-center justify-center select-none my-4">
      <i :class="`iconfont icon-${icon} text-primary-500 text-3xl`"></i>
    </div>

    <ul class="flex flex-col space-y-2 flex-auto">
      <slot></slot>

      <div class="flex-auto"></div>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    icon: {
      type: String,
      default: "logo"
    },
    isMenuShow: {
      type: Boolean,
      required: true
    }
  },

  setup() {
    return {

    };
  }
});
</script>
