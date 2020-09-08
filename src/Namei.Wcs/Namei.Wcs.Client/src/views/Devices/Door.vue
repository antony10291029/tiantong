<template>
  <div
    class="panel-block"
    style="height: 60px"
  >
      <div class="label"># {{door.id}}</div>

      <div class="is-flex-auto"></div>

      <span
        class="tag is-light"
        style="margin-right: 0.5rem"
        v-class:is-info="isRequesting"
      >
        <span v-if="isRequesting">请求中</span>
        <span v-else>无请求</span>
      </span>

      <a
        v-if="isOpened"
        class="tag is-light is-info"
      >
        关门
      </a>

      <a
        v-else
        class="tag is-light"
      >
        开门
      </a>
  </div>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'

export default Vue.extend({
  name: 'Door',

  props: {
    door: {
      type: Object,
      required: true
    }
  },

  computed: {
    isRequesting () {
      return !!(this.door as any ).taskId
    },

    isOpened () {
      return (this.door as any).isOpened
    }
  }
})
</script>
