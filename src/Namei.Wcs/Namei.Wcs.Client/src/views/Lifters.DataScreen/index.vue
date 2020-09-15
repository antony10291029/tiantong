<template>
  <div style="padding: 1.25rem; overflow: auto; width: 100%; padding-bottom: 0">
    <div class="columns" style="height: 100%">
      <div class="column is-flex is-flex-column" style="height: 100%">
        <div class="box">
          <div style="height: 0.5rem"></div>

          <div class="columns">
            <div class="column">
              <article class="notification is-primary">
                <div class="notification-body">
                  <p class="title is-4 is-flex">
                    <span>今日入库</span>
                    <span class="is-flex-auto"></span>
                    <span>230</span>
                  </p>
                </div>
              </article>
            </div>

            <div class="column">
              <article class="notification is-success">
                <div class="notification-body">
                  <p class="title is-4 is-flex">
                    <span>今日出库</span>
                    <span class="is-flex-auto"></span>
                    <span>145</span>
                  </p>
                </div>
              </article>
            </div>

            <div class="column">
              <article class="notification is-info">
                <div class="notification-body">
                  <p class="title is-4 is-flex">
                    <span>今日调库</span>
                    <span class="is-flex-auto"></span>
                    <span>88</span>
                  </p>
                </div>
              </article>

            </div>
          </div>

          <div class="columns">
            <div class="column is-8">
             <ScatterPlotChart style="margin-bottom: -1rem" />
            </div>

            <div class="column">
              <PieChart />
            </div>
          </div>

        </div>

        <div class="box is-flex-auto" style="padding-bottom: 0">
          <LineChart />
        </div>
      </div>

      <div class="column is-narrow">
        <div class="panel" style="height: 100%; margin-bottom: 0; overflow: auto">
          <p class="panel-heading is-size-5 is-flex is-vcentered">
            <span>正在执行任务数：</span>

            <span class="tag is-light is-size-6">
              10
            </span>
          </p>
          <a
            class="panel-block is-flex"
            style="height: 60px; padding-left: 20px; padding-right: 20px"
            v-for="key in 20" :key="key"
          >
            <div>
              <span>
                状态：
              </span>
              <span class="tag is-info is-light">
                放货完成
              </span>
            </div>

            <span style="width: 24px"></span>

            <div>
              <span>
                托盘码：
              </span>
              <span class="tag is-info is-light">
                待扫描
              </span>
            </div>

            <span style="width: 24px"></span>

            <div>
              <span>
                楼层：
              </span>
              <span class="tag is-info is-light">
                待扫描
              </span>
            </div>
          </a>
          <a
            class="panel-block has-background-white"
            style="max-height: 0; cursor: default"
          >
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import BarChart from './BarChart.vue'
import ScatterPlotChart from './ScatterPlotChart.vue'
import LineChart from './LineChart.vue'
import PieChart from './PieChart.vue'
import BrokenLineChart from './BrokenLineChart.vue'

export default Vue.extend({
  name: 'LiftersDataScreen',  

  components: {
    BarChart,
    PieChart,
    LineChart,
    ScatterPlotChart,
    BrokenLineChart
  },

  mounted () {

  }
})
</script>
