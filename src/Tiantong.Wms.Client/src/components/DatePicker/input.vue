<template>
  <input
    type="text"
    :class="['input', 'is-' + color]"
    :placeholder="placeholder"
    :value="value"
  />
</template>

<script>
import mixin from './mixin'

export default {
  mixins: [mixin]
}
</script>
