<template>
  <div class="is-flex is-flex-column" style="padding: 1.25rem">
    <div class="box">
      <p class="title is-4">
        今日数据
      </p>

      <hr>

      <div style="height: 0.5rem"></div>

      <div class="columns">
        <div class="column">
          <article class="notification is-primary">
            <div class="notification-body">
              <p class="title is-3 is-flex">
                <span>入库任务</span>
                <span class="is-flex-auto"></span>
                <span>123</span>
              </p>
            </div>
          </article>
        </div>

        <div class="column">
          <article class="notification is-success">
            <div class="notification-body">
              <p class="title is-3 is-flex">
                <span>出库任务</span>
                <span class="is-flex-auto"></span>
                <span>123</span>
              </p>
            </div>
          </article>

        </div>

        <div class="column">
          <article class="notification is-info">
            <div class="notification-body">
              <p class="title is-3 is-flex">
                <span>调库任务</span>
                <span class="is-flex-auto"></span>
                <span>123</span>
              </p>
            </div>
          </article>

        </div>
      </div>
    </div>

    <div class="box">

      <p class="title is-4">
        7日数据
      </p>

      <hr>

      <div class="columns">
        <div class="column">
          <BarChart />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import BarChart from './BarChart.vue'

export default Vue.extend({
  name: 'LiftersDataScreen',

  components: {
    BarChart
  },

  mounted () {

  }
})
</script>
