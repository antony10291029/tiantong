<template>
  <div
    class="panel-block"
    style="height: 60px"
  >
      <div class="label"># {{id}}</div>

      <div class="is-flex-auto"></div>

      <span
        class="tag is-light"
        style="margin-right: 0.5rem"
      >
        无请求
      </span>

      <a class="tag is-light is-info">开门</a>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'Door',

  props: {
    id: {
      type: String,
      required: true
    }
  }
})
</script>
