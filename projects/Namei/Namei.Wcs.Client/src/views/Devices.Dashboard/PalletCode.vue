<template>
  <a
    v-if="hasPalletCode"
    :class="`tag is-${color}`"
    @click.right.prevent="$emit('barcode-click')"
  >
    {{code}}
  </a>
  <span v-else
    class="tag"
  >
    无托盘
  </span>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    color: {
      type: String,
      default: "info"
    },
    code: {
      type: String,
      required: false
    }
  },

  computed: {
    hasPalletCode (): boolean {
      return !!parseInt(this.code ?? "0") && (this.code as any).length === 6;
    }
  }
});
</script>
