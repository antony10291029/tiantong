<template>
  <div
    :class="`
      container bg-dark-800 h-full
      flex-shrink-0 shadow
      border-r border-dark-900
      overflow-y-auto overflow-x-hidden
      sm:block sm:w-72
      ${ isMenuShow ? 'w-screen' : 'hidden' }
    `"
  >
    <div class="p-4 py-3 hidden sm:flex items-center select-none">
      <i :class="`iconfont icon-${icon} text-primary-500 text-2xl`"></i>

      <p class="text-grey-200 text-lg ml-3">
        <slot name="title"></slot>
      </p>
    </div>

    <p class="mb-4 sm:hidden"></p>

    <ul class="flex flex-col space-y-1 flex-auto">
      <slot></slot>

      <div class="flex-auto"></div>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

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
