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
      tooltip: {
          trigger: 'axis',
          axisPointer: {            // 坐标轴指示器，坐标轴触发有效
              type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
          }
      },
      legend: {

      },
      grid: {
      },
      xAxis: [
          {
              type: 'category',
              data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
          }
      ],
      yAxis: [
          {
              type: 'value'
          }
      ],
      series: [
          {
              type: 'bar',
              data: [320, 332, 301, 334, 390, 330, 320]
          },
          {
              type: 'bar',
              stack: '广告',
              data: [120, 132, 101, 134, 90, 230, 210]
          },
          {
              type: 'bar',
              stack: '广告',
              data: [220, 182, 191, 234, 290, 330, 310]
          },
          {
              type: 'bar',
              stack: '广告',
              data: [150, 232, 201, 154, 190, 330, 410]
          },
          {
              type: 'bar',
              data: [862, 1018, 964, 1026, 1679, 1600, 1570],
              markLine: {
                  lineStyle: {
                      type: 'dashed'
                  },
                  data: [
                      [{type: 'min'}, {type: 'max'}]
                  ]
              }
          },
          {
              type: 'bar',
              barWidth: 5,
              stack: '搜索引擎',
              data: [620, 732, 701, 734, 1090, 1130, 1120]
          },
          {
              type: 'bar',
              stack: '搜索引擎',
              data: [120, 132, 101, 134, 290, 230, 220]
          },
          {
              type: 'bar',
              stack: '搜索引擎',
              data: [60, 72, 71, 74, 190, 130, 110]
          },
          {
              type: 'bar',
              stack: '搜索引擎',
              data: [62, 82, 91, 84, 109, 110, 120]
          }
      ]
    };


    chart.setOption(option as any)
  }
})
</script>
