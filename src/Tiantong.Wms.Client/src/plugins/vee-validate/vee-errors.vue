<template>
  <div>
    <p
      v-for="error in errors"
      class="help is-danger"
      v-text="error"
      :key="error"
    />
  </div>
</template>

<script lang="ts">
export default {
  name: 'vee-errors',
  props: {
    errors: Array
  }
}
</script>
