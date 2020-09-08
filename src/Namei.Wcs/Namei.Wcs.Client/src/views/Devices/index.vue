<template>
  <section
    class="is-flex is-flex-column"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="columns">
      <div
        class="column is-narrow"
        v-for="key in 3" :key="key"
      >
        <Lifter />
      </div>
    </div>

    <Doors />
  </section>
</template>

<script lang="ts">
import Vue from 'vue'
import Doors from './Doors.vue'
import Lifter from './Lifter.vue'

export default Vue.extend({
  name: 'Devices',
  components: {
    Doors,
    Lifter,
  }
})
</script>
