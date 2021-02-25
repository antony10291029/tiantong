<template>
  <section class="columns">
    <div class="column is-narrow">
      <div class="panel" style="width: 320px">
        <p class="panel-heading">
          1F 自动门
        </p>
        <Door
          v-for="id in ['101', '102', '103', '104', '105', '106', '107']" :key="id"
          :door="doors[id]"
        />
      </div>
    </div>

    <div class="column is-narrow">
      <div class="panel" style="width: 320px">
        <p class="panel-heading">
          2F 自动门
        </p>
        <Door
          v-for="id in ['201', '202']" :key="id"
          :door="doors[id]"
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
  },

  props: {
    doors: {
      type: Object,
      required: true
    }
  }
})
</script>
