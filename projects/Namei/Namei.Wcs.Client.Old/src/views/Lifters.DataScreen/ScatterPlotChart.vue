<template>
  <div ref="chart" style="height: 320px; width: 100%"></div>
</template>

<script lang="ts">
import Vue from 'vue'
import echarts from 'echarts'

export default Vue.extend({
  name: 'ScatterPlotChart',

  mounted () {
    const chart = echarts.init(this.$refs['chart'] as any)
    const option = {
      legend: {
          data: ['入库', '出库', '调库'],
          left: 'center'
      },
      xAxis: {},
      yAxis: {},
      series: [
        {
          name: '入库',
          type: 'scatter',
          symbolSize: 10,
          data: [
            [10.0, 8.04],
            [8.0, 6.95],
            [13.0, 7.58],
            [9.0, 8.81],
            [11.0, 8.33],
            [14.0, 9.96],
            [6.0, 7.24],
            [4.0, 4.26],
            [12.0, 10.84],
            [7.0, 4.82],
            [5.0, 5.68]
          ],
        },
        {
          name: '出库',
          type: 'scatter',
          symbolSize: 10,
          data: [
            [12.0, 9.04],
            [1.0, 7.95],
            [6.0, 12.58],
            [5.0, 3.81],
            [17, 2.33],
            [14, 8.96],
          ],
        },
        {
          name: '调库',
          symbolSize: 10,
          type: 'scatter',
          data: [
            [12.0 + 3, 9.04 - 1],
            [1.0, 7.95 + 4],
            [6.0, 12.58 - 5],
            [5.0, 3.81 + 2],
            [17, 2.33 + 6],
            [14, 8.96 - 5],
          ],
        },
      ]
    }

    chart.setOption(option)
    const resize = () => chart.resize()
    window.addEventListener('resize', resize)
    this.$once('hook:destroyed', () => window.removeEventListener('resize', resize))
  }
})
</script>
