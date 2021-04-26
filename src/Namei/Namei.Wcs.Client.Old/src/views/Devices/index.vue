<template>
  <AsyncLoader
    :handler="getDataSource"
    class="is-flex is-flex-column"
    style="padding: 1.25rem; overflow: auto"
    v-slot="{ isLoading }"
  >
    <template v-if="!isLoading">
      <div class="columns">
        <div
          class="column is-narrow"
          v-for="id in ['1', '2', '3']" :key="id"
        >
          <Lifter :lifterId="id" :lifter="lifters[id]" :doors="doors" />
        </div>
      </div>

      <Doors :doors="doors" />
    </template>
    <router-view />
  </AsyncLoader>
</template>

<script>
import Vue from 'vue'
import Doors from './Doors.vue'
import Lifter from './Lifter.vue'
import domain from '@/providers/contexts/domain'

export default Vue.extend({
  name: 'Devices',
  components: {
    Doors,
    Lifter,
  },

  data: () => ({
    lifters: [{
      isWorking: false,
      isAlerting: false,
      floors:[3, 2, 1, 0].map(() => ({
        isDoorOpened: false,
        isExported: false,
        isImportAllowed: false,
        isScanned: false,
      }))
    }],

    doors: [{
      id: '',
      type: 'automated',
      IsOpened: false,
      taskId: '',
      count: 0,
    }],

    isInitialized: [false, false],

    isPending: false,

    interval: {}
  }),

  computed: {
    isDataInitialized () {
      return this.isInitialized[0] && this.isInitialized[1]
    }
  },

  methods: {
    async getLifters () {
      const response = await domain.post('/lifters/states')

      this.lifters = response.data
    },

    async getDoors () {
      const response = await domain.post('/doors/states')

      this.doors = response.data
    },

    async getDataSource () {
      if (this.isPending == true) {
        return
      }

      try {
        this.isPending = true
        await Promise.all([this.getLifters(), this.getDoors()])
      } finally {
        this.isPending = false
      }
    }
  },

  created () {
    setTimeout(() => {
      this.interval = setInterval(this.getDataSource, 1000)
    }, 1000)
  },

  beforeDestroy () {
    clearInterval(this.interval)
  }
})
</script>
