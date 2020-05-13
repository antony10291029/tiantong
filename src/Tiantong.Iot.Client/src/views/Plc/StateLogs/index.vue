<template>
  <AsyncLoader
    :handler="getStates"
    #default="{ isStatesPending }"
    style="padding: 1.25rem"
  >
    <AsyncLoader
      v-if="!isStatesPending"
      :handler="getLogs"
      #default="{ isPending }"
    >
      <template v-if="!isPending">
        <Table
          colspan="6"
          class="table is-centered is-bordered is-fullwidth"
        >
          <thead slot="head">
            <th>数据点</th>
            <th>地址</th>
            <th>类型</th>
            <th>操作</th>
            <th>数据</th>
            <th>时间</th>
          </thead>
          <tbody
            slot="body"
            v-if="logs.data.length > 0"
          >
            <tr v-for="log in logs.data" :key="log.id">
              <td>{{states[log.state_id].name}}</td>
              <td>{{states[log.state_id].address}}</td>
              <td>{{states[log.state_id].type}}</td>
              <td>{{log.operation}}</td>
              <td>{{log.value}}</td>
              <td>{{log.created_at.split('.')[0]}}</td>
            </tr>
          </tbody>
          <tbody v-else>
            <EmptyRow :colspan="6" />
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
import { PlcState, PlcStateLog } from '@/entities'

@Component({
  name: 'PlcStateLogs',
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  states: { [key: string]: PlcState } = {}

  page = 1

  pageSize = 15

  logs = {
    meta: {
      page: 1,
      pageSize: 1,
      total: 1,
    },
    data: []
  }

  async getStates () {
    let response = await axios.post('/plcs/states/all', {
      plc_id: this.plcId
    })

    this.states = {}
    response.data.forEach((state: PlcState) => {
      this.$set(this.states, state.id, state)
    })
  }

  async getLogs () {
    let response = await axios.post('/plcs/state-logs/paginate', {
      plc_id: this.plcId,
      page: this.page,
      page_size: this.pageSize,
    })

    this.logs = response.data
  }

  async changePage (page: number) {
    this.page = page
    await this.getLogs()
  }
}
</script>
