<template>
  <router-link
    custom :to="route"
    #="{ isExactActive, navigate }"
  >
    <a
      :class="`
        relative flex flex-row items-center
        h-11 pr-6
        border-l-2
        focus:outline-none
        hover:bg-dark-700 hover:text-grey-200
        ${isExactActive ? 'bg-dark-700 text-info-500 border-primary-500' : 'border-transparent' }
      `"
      @click="navigate"
      @click.capture="$emit('click')"
    >
      <span class="ml-4">
        <i :class="`iconfont icon-${icon}`"></i>
      </span>

      <span class="ml-2 text-sm tracking-wide truncate">
        {{text}}
      </span>

      <slot></slot>
    </a>
  </router-link>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

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
