<template>
  <div class="app-notifications is-flex is-flex-column is-vcentered">
    <Notification
      v-for="item in items" :key="item.id"
      @close="remove(item.id)"
      v-bind="item"
    ></Notification>
  </div>
</template>

<script>
import Vue from 'vue'
import Notification from './Notification'

export default {
  name: 'Notifications',
  components: {
    Notification
  },
  data () {
    return {
      count: 1,
      items: []
    }
  },
  methods: {
    async add (text, type = 'success', duration = 3333) {
      this.items.push({ text, type, duration, id: this.count++ })
    },
    remove (id) {
      const index = this.items.findIndex(item => item.id === id)
      if (index !== -1) {
        this.items.splice(index, 1)
      }
    }
  },
  created () {
    var notify = {
      add: this.add,
      info: (text, duration = 3333) => this.add(text, 'info', duration),
      link: (text, duration = 3333) => this.add(text, 'link', duration),
      danger: (text, duration = 3333) => this.add(text, 'danger', duration),
      success: (text, duration = 3333) => this.add(text, 'success', duration),
      warning: (text, duration = 3333) => this.add(text, 'warning', duration),
    }

    Vue.prototype.$notify = notify
  }
}
</script>
