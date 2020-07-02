<template>
  <AsyncLoader
    :handler="getDevices"
    class="is-flex-auto"
    style="overflow: auto; padding: 1.25rem"
  >
    <div class="columns is-variable is-3">
      <div class="column is-narrow"  style="width: 280px">
        <nav class="panel">
          <p class="panel-heading is-flex">
            <span>设备</span>
            <span class="is-flex-auto"></span>
            <router-link
              class="is-size-7"
              :to="`${baseURL}/devices/create`"
            >添加</router-link>
          </p>
          <template v-if="devices.length > 0">
            <router-link
              v-for="device in devices" :key="device.id"
              class="panel-block"
              :to="`${baseURL}/devices/${device.id}`"
            >
              <span>{{device.name}}</span>
            </router-link>
          </template>
          <template v-else>
            <a class="panel-block is-centered is-flex">
              <span class="is-italic has-text-grey">无</span>
            </a>
          </template>
        </nav>
      </div>

      <div class="column">
        <router-view
          :key="$route.params.deviceId"
          :baseURL="`${baseURL}/devices`"
          :projectId="projectId"
          @refresh="getDevices"
        />
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'Devices'
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  projectId!: number

  devices: any[] = []

  async getDevices () {
    const response = await domain.post('/devices/search', {
      project_id: this.projectId
    })

    this.devices = response.data
  }
}
</script>
