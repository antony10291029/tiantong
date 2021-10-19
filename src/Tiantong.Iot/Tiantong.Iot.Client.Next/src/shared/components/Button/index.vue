<template>
  <button
    :class="`
      cursor-pointer
      flex px-4
      items-center justify-center
      ring-2 ring-transparent bg-${type}-700
      hover:bg-${type}-600 hover:text-dark-300
      active:bg-${type}-700 active:text-dark-400
      disabled:opacity-50 disabled:cursor-not-allowed
    `"
    style="padding-top: calc(0.375rem + 1px);padding-bottom: calc(0.375rem + 1px); "
  >
    <slot></slot>
  </button>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    type: {
      type: String,
      default: "info"
    }
  },

  setup() {
    return {

    };
  }
});
</script>

<style>
/* bg-info-600 */
/* bg-info-600 */
/* bg-info-700 */

/* bg-link-500 */
/* bg-link-600 */
/* bg-link-700 */

/* bg-danger-500 */
/* bg-danger-600 */
/* bg-danger-700 */

/* bg-primary-500 */
/* bg-primary-600 */
/* bg-primary-700 */

/* bg-warning-500 */
/* bg-warning-600 */
/* bg-warning-700 */

/* bg-success-500 */
/* bg-success-600 */
/* bg-success-700 */
</style>
