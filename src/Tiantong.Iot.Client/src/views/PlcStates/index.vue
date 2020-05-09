<template>
  <AsyncLoader :handler="getStates">
    <div>
      <a class="button is-info is-small">
        添加
      </a>
    </div>

    <div style="height: 1.25rem"></div>

    <table class="table is-bordered is-fullwidth">
      <thead>
        <th>数据名称</th>
        <th>备注</th>
        <th>数据地址</th>
        <th>数据类型</th>
        <th>长度</th>
        <th>心跳写入</th>
        <th>开启日志</th>
        <th>操作</th>
      </thead>
      <tbody>
        <tr v-for="state in states" :key="state.id">
          <td>{{state.name}}</td>
          <td>{{state.comment}}</td>
          <td>{{state.address}}</td>
          <td>{{state.type}}</td>
          <td>{{state.length}}</td>
          <td></td>
          <td></td>
          <td>
            <a class="is-size-7">删除</a>
          </td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '../../providers/axios'

@Component({
  name: 'PlcStates'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  states: object[] = []

  async getStates () {
    let response = await axios.post('plcs/states/all', {
      plc_id: this.plcId
    })

    this.states = response.data
  }

  async created () {

  }
}
</script>
