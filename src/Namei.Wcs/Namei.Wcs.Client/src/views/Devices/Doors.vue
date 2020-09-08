<template>
  <section class="columns">
    <div class="column is-narrow">
      <div class="panel" style="width: 280px">
        <p class="panel-heading">
          1F 自动门
        </p>
        <Door
          v-for="key in ['101', '102']" :key="key"
          :id="key"
        />
      </div>
    </div>

    <div class="column is-narrow">
      <div class="panel" style="width: 280px">
        <p class="panel-heading">
          2F 自动门
        </p>
        <Door
          v-for="key in ['201', '202', '203']" :key="key"
          :id="key"
        />
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import Vue from 'vue'
import Door from './Door.vue'

export default Vue.extend({
  name: 'Doors',

  components: {
    Door
  }
})
</script>
