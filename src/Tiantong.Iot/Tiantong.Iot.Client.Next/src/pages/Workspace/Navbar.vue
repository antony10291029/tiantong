<template>
  <nav
    :class="`
      sm:hidden
      bg-dark-800
      w-screen flex-none
      border-b shadow
      border-dark-900
    `"
  >
    <div class="mx-auto px-2 py-0.5 sm:px-6">
      <div class="relative flex items-center justify-between h-12">
        <div class="absolute inset-y-0 left-0 flex items-center">
          <button
            type="button"
            class="
              inline-flex items-center justify-center
              p-1 rounded-md w-10
              hover:text-white hover:bg-dark-700
              focus:outline-none focus:ring-1 focus:ring-inset focus:ring-gray-400
              sm:hidden
            "
            @click="toggleMenu(!isMenuShow)"
          >
            <i class="iconfont icon-navbar-menu text-xl"></i>
          </button>
        </div>
        <div class="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start">
          <div class="flex-shrink-0 flex items-center">
            <img class="w-6" src="../../assets/logo.svg">
            <p class="text-white text-2xl ml-3">LinkFlow</p>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

export default defineComponent({
  props: {
    isMenuShow: {
      type: Boolean,
      required: true
    },

    toggleMenu: {
      type: Function as PropType<(value?: boolean) => void>,
      required: true
    }
  },

  setup() {
    return {

    };
  }
});
</script>
