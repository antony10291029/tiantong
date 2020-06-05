<template>
  <div style="min-height: 280px; padding: 1.25rem; overflow: hidden">
    <div ref="chart" style="width: 100%; height: 480px; margin-bottom: 0"></div>
    <table
      v-if="group"
      class="table is-bordered is-centered is-fullwidth is-nowrap"
      style="margin-top: -4rem"
    >
      <thead>
        <th style="width: 1px">#</th>
        <th style="width: 1px">日期</th>
        <th>科目</th>
        <th>借方</th>
        <th>摘要</th>
      </thead>
      <tbody>
        <tr v-for="(detail, key) in group.items" :key="detail.id">
          <td>{{key + 1}}</td>
          <td>
            {{detail.period}}
          </td>
          <td>
            {{detail.glAccountLongName}}
          </td>
          <td>
            {{detail.basePostedDr}}
          </td>
          <td style="text-align: left">
            {{detail.comments}}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import echarts from "echarts";

@Component({
  name: "ProjectChart"
})
export default class extends Vue {
  @Prop({ required: true })
  categories!: any[]

  @Prop({ required: true })
  details!: any[]

  detailsGroups = new Array<any>()

  isModalShow = false

  group = null

  mounted() {
    this.detailsGroups = getDetailsGroups(this.details)

    const chart = echarts.init(this.$refs.chart as any)

    chart.on('click', (event: any) => {
      this.isModalShow = true
      this.group = this.detailsGroups[event.dataIndex]
    })

    const options = getOptions(this.detailsGroups)
    chart.setOption(options)
  }
}

function getDetailsGroups (details: any): any {
  const group = {} as any

  details.forEach((detail: any) => {
    const groupCode = detail.glAccountCode
    if (!group[groupCode]) {
      const nameSplits = detail.glAccountLongName.split('_')

      group[groupCode] = {
        code: groupCode,
        name: nameSplits[1] ?? nameSplits[0],
        total: 0,
        items: []
      }
    }

    group[groupCode].total += detail.basePostedDr
    group[groupCode].items.push(detail)
  })
  const keys = Object.keys(group)
  const groups = keys.map(key => group[key])

  groups.sort((a, b) => b.total - a.total)

  return groups
}

function getTotal (details: any) {
  return details.reduce((s: number, d: any) => s += d.basePostedDr, 0)
}

function getOptions (groups: any[]): any {
  return {
    tooltip: {
      trigger: "item",
      formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
    legend: {
      left: 10,
      data: groups.map(group => group.name),
    },
    series: [
      {
        name: '科目',
        type: "pie",
        radius: ["40%", "55%"],
        avoidLabelOverlap: false,
        label: {
          show: false,
          position: 'center',
          formatter: "{b} \n {c}({d}%)",
        },
        emphasis: {
          label: {
            show: true,
            fontSize: '14',
            fontWeight: 'bold'
          }
        },
        labelLine: {
          show: false
        },
        data: groups.map(group => ({
          value: group.total.toFixed(2),
          name: group.name
        }))
      }
    ]
  }
}
</script>
