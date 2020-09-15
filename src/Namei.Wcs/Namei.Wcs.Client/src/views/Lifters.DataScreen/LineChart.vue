<template>
  <div ref="chart" style="height: 100%"></div>
</template>

<script lang="ts">
import Vue from 'vue'
import echarts from 'echarts'

export default Vue.extend({
  name: 'PieChart',

  mounted () {
    const chart = echarts.init(this.$refs['chart'] as any)
    const option = {
      title: {
        text: '近期数据'
      },
      legend: {},
      tooltip: {
        trigger: 'axis',
        showContent: false
      },
      dataset: {
        source: [
          ['product', '9-12', '9-13', '9-14', '9-15', '9-16', '9-17'],
          ['入库', 86.5, 92.1, 85.7, 83.1, 73.4, 55.1],
          ['出库', 24.1, 67.2, 79.5, 86.4, 65.2, 82.5],
          ['调库', 55.2, 67.1, 69.2, 72.4, 53.9, 39.1]
        ]
      },
      xAxis: {type: 'category'},
      yAxis: {gridIndex: 0},
      series: [
        { type: 'line', smooth: true, seriesLayoutBy: 'row' },
        { type: 'line', smooth: true, seriesLayoutBy: 'row' },
        { type: 'line', smooth: true, seriesLayoutBy: 'row' },
        { type: 'line', smooth: true, seriesLayoutBy: 'row' },
      ]
    }

    chart.setOption(option as any)

    const resize = () => chart.resize()
    window.addEventListener('resize', resize);
    this.$once('hook:destroyed', () => window.removeEventListener('resize', resize))
  }
})
</script>
