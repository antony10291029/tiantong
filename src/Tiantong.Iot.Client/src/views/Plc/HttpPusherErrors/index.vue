<template>
  <AsyncLoader
    :handler="getPushers"
    #default="{ isPushersPending }"
    style="padding: 1.25rem"
  >
    <AsyncLoader
      v-if="!isPushersPending"
      :handler="getLogs"
      #default="{ isPending }"
    >
      <template v-if="!isPending">
        <table class="table is-centered is-bordered is-fullwidth">
          <thead>
            <th>推送名</th>
            <th>URL</th>
            <th>详情</th>
            <th>时间</th>
          </thead>
          <tbody>
            <tr v-for="log in logs.data" :key="log.id">
              <td>{{pushers[log.pusher_id].name}}</td>
              <td>{{pushers[log.pusher_id].url}}</td>
              <td>{{log.message}}</td>
              <td>{{log.created_at.split('.')[0]}}</td>
            </tr>
          </tbody>
        </table>

        <div style="height: 1.25rem"></div>

        <Pagination
          v-bind="logs.meta"
          @change="changePage"
        />
      </template>
    </AsyncLoader>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import StateLogs from './Logs.vue'
import axios from '@/providers/axios'
import { PlcState, PlcStateLog, HttpPusher } from '@/entities'

@Component({
  name: 'PlcStateLogs',
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  states: { [key: string]: PlcState } = {}

  page = 1

  pageSize = 15

  pushers: { [key: string]: HttpPusher } = {}

  logs = {
    meta: {
      page: 1,
      pageSize: 1,
      total: 1,
    },
    data: []
  }

  get pusherIds () {
    return Object.values(this.pushers).map(pusher => pusher.id)
  }

  async getPushers () {
    let response = await axios.post('/plcs/http-pushers/all', {
      plc_id: this.plcId
    })

    this.pushers = {}
    response.data.forEach((pusher: HttpPusher) => {
      this.$set(this.pushers, pusher.id, pusher)
    })
  }

  async getLogs () {
    let response = await axios.post('/plcs/http-pusher-errors/paginate', {
      ids: this.pusherIds,
      page: this.page,
      page_size: this.pageSize
    })

    this.logs = response.data
  }

  async changePage (page: number) {
    this.page = page
    await this.getLogs()
  }
}
</script>
