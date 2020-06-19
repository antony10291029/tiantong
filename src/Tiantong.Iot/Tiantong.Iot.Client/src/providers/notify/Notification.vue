<template>
  <div :class="`notification is-${type}`">
    <template v-if="type">
      <span :class="'icon has-text-' + type">
        <i :class="'iconfont icon-' + type" style="font-size: 1.75rem"></i>
      </span>
      <span style="width: 1rem"></span>
    </template>
    <span>{{text}}</span>
    <span class="is-flex-auto" style="min-width: 1rem"></span>
    <span class="is-flex is-vcentered">
      <button @click="close" class="delete"></button>
    </span>
  </div>
</template>

<script>
import Vue from 'vue'
import Component from 'vue-class-component'

export default {
  name: 'NotificationItem',
  props: {
    type: {
      type: String,
      default: 'success'
    },
    duration: {
      type: Number,
      default: 3333
    },
    text: {
      type: String,
      default: '数据已更新'
    }
  },
  methods: {
    close () {
      this.$emit('close')
    }
  },
  created () {
    if (this.duration) {
      setTimeout(() => this.close(), this.duration)
    }
  }
}
</script>
