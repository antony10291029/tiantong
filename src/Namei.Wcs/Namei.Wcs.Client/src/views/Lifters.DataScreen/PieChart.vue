<template>
  <div ref="chart" style="height: 100%; width: 100%;"></div>
</template>

<script lang="ts">
import Vue from 'vue'
import echarts from 'echarts'

export default Vue.extend({
  name: 'PieChart',

  mounted () {
    const chart = echarts.init(this.$refs['chart'] as any)
    const options = {
      title: {
        text: '出入库比率',
        left: 'center'
      },
      series: [
        {
          name: '访问来源',
          type: 'pie',
          radius: '55%',
          center: ['50%', '60%'],
          data: [
            {value: 335, name: '入库'},
            {value: 310, name: '出库'},
            {value: 234, name: '调库'},
          ],
          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
          }
        }
      ]
    }

    chart.setOption(options as any)

    const resize = () => chart.resize()
    window.addEventListener('resize', resize);
    this.$once('hook:destroyed', () => window.removeEventListener('resize', resize))
  }
})
</script>
