<template>
  <div
    class="is-flex is-flex-column"
    style="overflow: auto;"
  >
    <div
      class="tabs"
      style="margin-bottom: 0; font-size: 1.125rem"
    >
      <ul>
        <li class="is-active"><a>系统状态</a></li>
      </ul>
    </div>
    <div
      class="is-flex-auto"
      style="padding: 1.25rem"
    >
      <div class="box">
        <table class="table is-nowrap is-bordered is-vcentered is-centered is-fullwidth">
          <thead>
            <th colspan="8">提升机状态</th>
          </thead>
          <tbody>
            <tr>
              <th>设备模式</th>
              <td style="min-width: 50px"></td>
              <th>工作状态</th>
              <td style="min-width: 50px"></td>
              <th>报警信息</th>
              <td style="min-width: 50px"></td>
              <th>心跳</th>
              <td style="min-width: 50px"></td>
            </tr>
            <tr>
              <th>升降平台状态</th>
              <td style="min-width: 50px"></td>
              <th>平台内输送机状态</th>
              <td style="min-width: 50px"></td>
              <th>平台托盘码</th>
              <td style="min-width: 50px"></td>
              <th>平台任务路径</th>
              <td style="min-width: 50px"></td>
            </tr>
          </tbody>
        </table>

        <div style="height: 1.25rem"></div>

        <table class="table is-nowrap is-fullwidth is-bordered is-vcentered is-centered">
          <thead>
            <tr>
              <th rowspan="2">楼层</th>
              <th colspan="3">B 段 - 输送机</th>
              <th colspan="3">A 段 - 输送机</th>
              <th colspan="4">A 段 - 工位</th>
            </tr>
            <tr>
              <th>状态</th>
              <th>托盘码</th>
              <th>路径</th>

              <th>状态</th>
              <th>托盘码</th>
              <th>路径</th>
              <th>终点</th>
              <th>扫码器</th>
              <th>工位</th>
              <th>AGC</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="key in 4" :key="key">
              <th>{{5 - key}}</th>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'Lifters'
})
export default class extends Vue {

}
</script>
