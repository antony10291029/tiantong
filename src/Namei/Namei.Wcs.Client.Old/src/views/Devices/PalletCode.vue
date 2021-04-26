<template>
  <span
    v-if="hasPalletCode"
    :class="`tag is-${color}`"
  >
    {{code}}
  </span>
  <span v-else
    class="tag"
  >
    无托盘
  </span>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  props: {
    color: {
      type: String,
      default: 'info'
    },
    code: {
      type: String,
      required: true
    }
  },

  computed: {
    hasPalletCode () {
      return parseInt(this.code) && (this.code as any).length == 6
    }
  }
})
</script>
