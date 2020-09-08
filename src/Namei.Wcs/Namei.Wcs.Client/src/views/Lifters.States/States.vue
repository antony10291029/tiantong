<template>
  <AsyncLoader :handler="getDataSource">
    <table class="table is-nowrap is-bordered is-vcentered is-centered is-fullwidth">
      <thead>
        <th colspan="6">
          {{text}}
        </th>
      </thead>
      <tbody>
        <tr>
          <th></th>
          <th>工作状态</th>
          <td style="min-width: 50px">
            <span
              v-if="!states.isWorking"
              class="tag is-success"
            >
              空闲中
            </span>
            <span
              v-else
              class="tag is-warning"
            >
              运行中
            </span>
          </td>
          <th>报警信息</th>
          <td style="min-width: 50px">
            <span
              v-if="!states.isAlarming"
              class="tag is-success"
            >
              未报警
            </span>
            <span
              v-else
              class="tag is-danger"
            >
              报警中
            </span>
          </td>
          <td></td>
        </tr>
      </tbody>

      <thead>
        <tr>
          <th style="width: 1px">楼层</th>
          <th>扫码状态</th>
          <th>请求取货</th>
          <th>允许放货</th>
          <th>虚拟安全门</th>
          <th>操作</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="floor in floors" :key="floor">
          <th>{{floor + 1}}</th>
          <td>
            <span
              v-if="states.floors[floor].isScanned"
              class="tag is-warning"
            >
              已扫码
            </span>
            <span
              v-else
              class="tag is-success"
            >
              未扫码
            </span>
          </td>
          <td>
            <span
              v-if="states.floors[floor].isExported"
              class="tag is-warning"
            >
              待取货
            </span>
            <span
              v-else
              class="tag is-success"
            >
              无请求
            </span>
          </td>
          <td>
            <span
              v-if="states.floors[floor].isImportAllowed"
              class="tag is-success"
            >
              允许放货
            </span>
            <span
              v-else
              class="tag is-warning"
            >
              不允许放货
            </span>
          </td>
          <td>
            <span
              v-if="!states.floors[floor].isDoorOpened"
              class="tag is-success"
            >
              已关闭
            </span>
            <span
              v-else
              class="tag is-warning"
            >
              已打开
            </span>
          </td>
          <td>
            <a>控制指令</a>
          </td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'LifterStates'
})
export default class extends Vue {
  @Prop({ required: true })
  lifterId!: string

  @Prop({ required: true })
  text!: string

  states = {
    isWorking: false,
    isAlarming: false,
    floors:[3, 2, 1, 0].map(() => ({
      isDoorOpened: false,
      isExported: false,
      isImportAllowed: false,
      isScanned: false,
    }))
  }

  interval: any

  floors = [3, 2, 1, 0]

  async getDataSource () {
    const response = await domain.post('/lifters/states', {
      lifter_id: this.lifterId
    })

    this.states = response.data
  }

  created () {
    this.interval = setInterval(() => this.getDataSource(), 1000);
  }

  beforeDestroy () {
    clearInterval(this.interval)
  }
}
</script>
