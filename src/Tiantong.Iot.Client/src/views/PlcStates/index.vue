<template>
  <AsyncLoader
    :handler="getStates"
    style="padding: 1.25rem;"
  >
    <div class="is-flex">
      <div style="min-width: 400px; max-width: 400px">
        <Table
          colspan="3"
          class="table is-bordered is-fullwidth is-nowrap is-hoverable"
        >
          <thead slot="head">
            <th>
              名称
            </th>
            <th>地址</th>
            <th>类型</th>
          </thead>
          <tbody
            slot="body"
            v-if="states.length > 0"
          >
            <tr
              v-class:is-active="state.id === stateId"
              v-for="state in states" :key="state.id"
              @click="handleStateClick(state)"
            >
              <td>{{state.name}}</td>
              <td>{{state.address}}</td>
              <td>
                {{state.type + (state.type === 'string' ? `(${state.length})` : '')}}
              </td>
            </tr>
          </tbody>
        </Table>

        <div style="height: 0.75rem"></div>

        <div>
          <router-link
            :to="`/plcs/${plcId}/states/create`"
            class="button is-info is-small"
          >
            添加
          </router-link>
        </div>
      </div>

      <div style="min-width: 1.25rem; max-width: 1.25rem"></div>

      <router-view
        class="is-flex-auto"
        :key="stateId"
        :plcId="plcId"
        :stateId="stateId"
        :baseURL="baseURL"
        @refresh="getStates"
        @delete="handleStateDeleted"
      />
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcStates',
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  baseURL!: string

  states: object[] = []

  get stateId () {
    return +this.$route.params.stateId
  }

  handleStateClick (state: any) {
    if (!state) return
    this.$router.push(`/plcs/${this.plcId}/states/${state.id}`)
  }

  handleStateDeleted () {
    this.$router.push(this.baseURL)
    this.getStates()
  }

  async getStates () {
    let response = await axios.post('plcs/states/all', {
      plc_id: this.plcId
    })

    this.states = response.data
    if (!this.stateId) {
      this.handleStateClick(this.states[0])
    }
  }
}
</script>
