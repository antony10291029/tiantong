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
            <i class="iconfont icon-navbar-menu text-lg"></i>
          </button>
        </div>
        <div class="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start">
          <div class="flex-shrink-0 flex items-center">
            <i :class="`iconfont icon-${icon} w-6 text-primary-500 text-2xl`"></i>
            <p class="text-lg ml-3">
              <slot name="title"></slot>
            </p>
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
    icon: {
      type: String,
      default: "logo"
    },
    isMenuShow: {
      type: Boolean,
      required: true
    },
    toggleMenu: {
      type: Function as PropType<(value?: boolean) => void>,
      required: true
    }
  }
});
</script>
