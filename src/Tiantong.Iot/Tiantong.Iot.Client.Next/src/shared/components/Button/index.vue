<template>
  <button
    :class="`
      button
      bg-${type}-700
      hover:bg-${type}-600
      active:bg-${type}-900
    `"
    style="padding-top: calc(0.375rem + 1px);padding-bottom: calc(0.375rem + 1px); "
  >
    <slot></slot>
  </button>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

type Type = "primary" | "info" | "success" | "link" | "warning" | "danger";

export default defineComponent({
  props: {
    type: {
      type: String as PropType<Type>,
      default: "info"
    }
  }
});
</script>

<style lang="postcss">
.button {
  @apply cursor-pointer;
  @apply flex px-4;
  @apply items-center justify-center;
  @apply ring-2 ring-transparent;
}

.button:hover {
  @apply text-dark-300;
}

.button:active {
  @apply text-dark-500;
}

.button:disabled {
  @apply opacity-50 cursor-not-allowed;
}

/* !! Tailwind Purge !! */

/* bg-info-600 hover:bg-info-600 active:bg-info-600 */
/* bg-info-600 hover:bg-info-600 active:bg-info-600 */
/* bg-info-700 hover:bg-info-700 active:bg-info-700 */

/* bg-link-500 hover:bg-link-500 active:bg-link-500 */
/* bg-link-600 hover:bg-link-600 active:bg-link-600 */
/* bg-link-700 hover:bg-link-700 active:bg-link-700 */

/* bg-danger-500 hover:bg-danger-500 active:bg-danger-500 */
/* bg-danger-600 hover:bg-danger-600 active:bg-danger-600 */
/* bg-danger-700 hover:bg-danger-700 active:bg-danger-700 */

/* bg-primary-500 hover:bg-primary-500 active:bg-primary-500 */
/* bg-primary-600 hover:bg-primary-600 active:bg-primary-600 */
/* bg-primary-700 hover:bg-primary-700 active:bg-primary-700 */

/* bg-warning-500 hover:bg-warning-500 active:bg-warning-500 */
/* bg-warning-600 hover:bg-warning-600 active:bg-warning-600 */
/* bg-warning-700 hover:bg-warning-700 active:bg-warning-700 */

/* bg-success-500 hover:bg-success-500 active:bg-success-500 */
/* bg-success-600 hover:bg-success-600 active:bg-success-600 */
/* bg-success-700 hover:bg-success-700 active:bg-success-700 */
</style>
