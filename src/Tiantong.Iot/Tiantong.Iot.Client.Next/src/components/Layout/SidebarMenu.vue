<template>
  <router-link
    custom :to="route"
    #="{ isExactActive, navigate }"
  >
    <a
      :class="`
        flex flex-col items-center justify-center
        w-full py-2 cursor-pointer
        focus:outline-none
        hover:text-dark-200
        ${isExactActive ? 'text-gray-300 border-gray-300' : 'text-gray-500' }
      `"
      @click="navigate()"
      @click.capture="$emit('click')"
    >
      <span class="mb-0.5">
        <i :class="`iconfont icon-${icon} text-xl`"></i>
      </span>

      <span class="text-sm">
        {{text}}
      </span>
    </a>
  </router-link>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    route: {
      type: [String, Object],
      required: true
    },
    text: {
      type: String,
      required: true
    },
    icon: {
      type: String,
      default: "menu-default"
    },
  },

  emits: [
    "click"
  ]
});
</script>
