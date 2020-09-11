<template>
  <div ref="chart" style="height: 400px"></div>
</template>

<script lang="ts">
import Vue from 'vue'
import echarts from 'echarts'

export default Vue.extend({
  name: 'BarChart',

  mounted () {
    const chart = echarts.init(this.$refs.chart as any)
    const option = {
      xAxis: {
        type: 'category',
        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
      },
      yAxis: {
        type: 'value'
      },
      series: [{
        data: [120, 200, 150, 80, 70, 110, 130],
        type: 'bar'
      }]
    }

    chart.setOption(option as any)
  }
})
</script>
