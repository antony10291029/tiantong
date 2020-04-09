<template>
  <td
    v-text="value"
    class="is-clickable"
  ></td>
</template>

<script>
import mixin from './mixin'

export default {
  mixins: [mixin],
}
</script>
