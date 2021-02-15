<template>
  <component
    :class="!isPending || 'is-loading'"
    @click="handleClick"
    :is="tag"
    :disabled="disabled || undefined"
  >
    <slot></slot>
  </component>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

export default defineComponent({
  name: "AsyncButton",

  props: {
    tag: {
      type: String,
      default: "a",
    },

    handler: {
      type: Function as PropType<() => void>,
      default: () => () => {}
    },

    disabled: {
      type: Boolean,
      default: false
    }
  },

  data: () => ({
    isPending: false
  }),

  methods: {
    async handleClick () {
      if (this.disabled) return;

      try {
        this.isPending = true;
        await this.handler();
      } finally {
        this.isPending = false;
      }
    }
  }
});
</script>
