<template>
  <AsyncLoader
    :handler="getPushers"
    #default="{ isPushersPending }"
  >
    <AsyncLoader
      v-if="!isPushersPending"
      :handler="getLogs"
      #default="{ isPending }"
    >
      <template v-if="!isPending">
        <Table
          colspan="6"
          class="table is-centered is-bordered is-fullwidth is-nowrap"
        >
          <thead slot="head">
            <th>推送名</th>
            <th>URL</th>
            <th>请求数据</th>
            <th>响应数据</th>
            <th>状态码</th>
            <th>时间</th>
          </thead>
          <tbody
            slot="body"
            v-if="logs.data.length > 0"
          >
            <tr v-for="log in logs.data" :key="log.id">
              <td>{{pushers[log.pusher_id].name}}</td>
              <td>{{pushers[log.pusher_id].url}}</td>
              <td>{{log.request}}</td>
              <td>{{log.response}}</td>
              <td>{{log.status_code}}</td>
              <td>{{log.created_at.split('.')[0]}}</td>
            </tr>
          </tbody>
        </Table>

        <div style="height: 0.75rem"></div>

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
import { PlcState, HttpPusher } from '@/entities'

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
    let response = await axios.post('/plcs/http-pusher-logs/paginate', {
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
