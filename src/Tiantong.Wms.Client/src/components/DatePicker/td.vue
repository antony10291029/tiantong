<template>
  <td
    style="min-width: 120px"
    v-text="value"
    :class="{
      'is-selectable': true,
      'is-changed': isActive
    }"
  />
</template>

<script>
import mixin from './mixin'

export default {
  mixins: [mixin]
}
</script>
