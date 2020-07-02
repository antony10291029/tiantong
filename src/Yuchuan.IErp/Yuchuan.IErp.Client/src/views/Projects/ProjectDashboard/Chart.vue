<template>
  <div class="box" style="width: 100%; height: 700px;">
    <div ref="chart" style="width: 100%; height: 100%;"></div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import echarts from 'echarts'

@Component({
  name: 'ProjectDashboardChart'
})
export default class extends Vue {
  @Prop({ required: true })
  states!: { [key: string]: any }

  @Prop({ required: true })
  devices!: any[]

  chart: any

  update () {
    this.chart.setOption(getOption(this.devices, this.states))
  }

  mounted () {
    this.chart = echarts.init(this.$refs.chart as any)
    const resize = () => this.chart.resize()

    window.addEventListener('resize', resize)
    this.$once('hook:destroyed', () => window.removeEventListener('resize', resize))

    this.update()
  }

  @Watch('states', { deep: true })
  handleStatesChange () {
    this.update()
  }
}

function getOption (devices: any[], states: any) {
  return {
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow'
      },
      formatter: function (params: any) {
        var tar

        if (params[1].value !== '-') {
          tar = params[1];
        } else {
          tar = params[0];
        }

        return tar.name + '<br/>' + tar.seriesName + ' : ' + tar.value;
      }
    },
    legend: {
      data: ['支出']
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
    },
    xAxis: {
        type: 'category',
        data: function () {
          let data = devices.map(device => device.name)

          while (data.length < 10) {
            data.push('')
          }

          return data
        }(),
        splitNumber: 10,
        interval: 1,
    },
    yAxis: {
      type: 'value',
      name: '楼层',
      nameTextStyle: {
        fontSize: '16'
      },
      splitLine: {
        show: false
      },
      min: 1,
      max: 11,
      interval: 1,
    },
    series: [
      {
        name: '辅助',
        type: 'bar',
        stack: '总量',
        itemStyle: {
          barBorderColor: 'rgba(0,0,0,0)',
          color: 'rgba(0,0,0,0)'
        },
        barMaxWidth: 60,
        emphasis: {
          itemStyle: {
            barBorderColor: 'rgba(0,0,0,0)',
            color: 'rgba(0,0,0,0)'
          }
        },
        data: devices.map(device => states[device.id].position ?? 1)
      },
      {
        name: '楼层',
        type: 'bar',
        stack: '总量',
        label: {
          show: false,
          position: 'top'
        },
        data: devices.map(device => 1)
      }
    ]
  };
}
</script>
