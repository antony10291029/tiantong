<template>
  <AsyncLoader
    :handler="getStates"
    style="padding: 1.25rem"
  >
    <div class="columns">
      <div class="column is-narrow">
        <div
          class="box is-paddingless"
          style="min-width: 400px"
        >
          <p
            class="is-flex has-border-bottom"
            style="padding: 0.5rem 0.75rem"
          >
            <span class="label">数据点</span>
            <span class="is-flex-auto"></span>
            <router-link
              class="is-size-6"
              :to="`${baseURL}/states/create`"
            >
              添加
            </router-link>
          </p>

          <Table
            colspan="4"
            class="table is-fullwidth is-nowrap is-hoverable is-radius"
          >
            <tbody
              slot="body"
              v-if="states.length > 0"
            >
              <router-link
                v-class:is-active="state.id === stateId"
                v-for="state in states" :key="state.id"
                @click="handleStateClick(state)"
                :to="`${baseURL}/states/${state.id}`"
                tag="tr"
              >
                <td class="is-centered">{{state.number}}</td>
                <td>{{state.name}}</td>
                <td>{{state.address}}</td>
                <td>
                  {{state.type + (state.type === 'string' ? `(${state.length * 2})` : '')}}
                </td>
                <td class="is-centered">
                  <span
                    class="icon"
                    v-if="state.is_collect"
                  >
                    <i class="iconfont icon-collect"></i>
                  </span>
                  <span
                    class="icon"
                    style="margin-left: 0.25rem"
                    v-if="state.is_heartbeat"
                  >
                    <i class="iconfont icon-heartbeat"></i>
                  </span>
                </td>
              </router-link>
            </tbody>
          </Table>
        </div>
      </div>

      <div class="column">
        <router-view
          :key="stateId"
          :plcId="plcId"
          :stateId="stateId"
          :baseURL="baseURL"
          @refresh="getStates"
          @delete="handleStateDeleted"
        />
      </div>
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
    if (!state || +this.$route.params.stateId === state.id) return
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
  }
}
</script>
