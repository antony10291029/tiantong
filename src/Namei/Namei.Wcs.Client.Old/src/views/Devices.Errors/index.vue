<template>
  <div style="padding: 1.25rem">
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading is-flex">
            <span>
              设备
            </span>
            <span class="is-flex-auto"></span>
            <router-link to="/devices/errors/create">
              添加
            </router-link>
          </div>

          <a
            v-for="device in devices" :key="device.id"
            class="panel-block"
          >
            {{device.name}}
          </a>
        </div>
      </div>

      <div class="column">
        <router-view @created="getDataSource"></router-view>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import domain from '@/providers/contexts/domain'

export default Vue.extend({
  name: 'DeviceErrors',

  data: () => ({
    devices: []
  }),

  methods: {
    async getDataSource () {
      const response = await domain.post('/devices/all')

      this.devices = response.data
    }
  },

  created () {
    this.getDataSource()
  }
})
</script>
