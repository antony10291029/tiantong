<template>
  <span :class="`tag is-${result.color}`">
    {{result.text}}
  </span>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'OrderStatusTag',

  props: {
    tag: {
      type: String,
      default: 'tag'
    },
    value: {
      type: String,
      required: true
    }
  },

  computed: {
    result () {
      switch (this.value) {
        case 'created': return {
          text: '未完成',
          color: 'warning'
        }
        case 'finished': return {
          text: '已入库',
          color: 'success'
        }
        case 'filed': return {
          text: '已归档',
          color: 'info'
        }
        default: return {
          text: '未知',
          color: 'danger'
        }
      }
    }
  }
})
</script>
