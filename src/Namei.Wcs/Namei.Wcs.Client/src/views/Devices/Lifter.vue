<template>
  <section class="box">
    <p class="title is-5" style="margin-bottom: 0.75rem">
      提升机
    </p>

    <div class="is-flex">
      <div
        class="has-background-primary is-radius"
        style="width: 40px; height: 440px; border-bottom-right-radius: 0"
      ></div>

      <!-- 输送机 -->
      <div>
        <div
          v-for="key in 4" :key="key"
          class="is-flex"
          style="height: 110px"
        >
          <div
            class="is-flex is-flex-column"
            style="width: 100px"
          >
            <div class="is-flex-auto"></div>

            <div class="is-flex is-centered">
              <span class="tag is-info is-light">
                无托盘
              </span>
            </div>

            <div
              class="has-background-primary is-flex is-vcentered is-centered has-text-white"
              style="height: 30px; width: 100px"
            >
              B 段
            </div>
          </div>

          <div
            class="is-flex is-flex-column"
            style="width: 100px"
          >
            <div class="is-flex-auto"></div>

            <div class="is-flex is-centered">
              <span class="tag is-info is-light">
                123456
              </span>
            </div>

            <div
              class="has-background-primary is-flex is-vcentered is-centered is-radius has-text-white"
              style="height: 30px; width: 100px;"
              v-style:border-top-left-radius="0"
              v-style:border-bottom-left-radius="0"
            >
              A 段
            </div>
          </div>

          <div
            class="is-flex is-flex-column"
            style="width: 100px"
          >
            <div class="is-flex-auto"></div>

            <div
              class="is-flex is-flex-column"
              style="width: 100px; margin-left: 0.5rem"
            >
              <div class="is-flex-auto"></div>

              <div class="is-flex is-centered">
                <a
                  class="tag is-success is-light"
                  style="margin-right: 0.125rem"
                >
                  取货
                </a>
                <a class="tag is-danger is-light">
                  放货
                </a>
              </div>

              <div class="is-flex is-centered is-vcentered" style="height: 30px">
                <a class="tag is-light">
                  AGC 请求通过
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'Lifter'  
})
</script>
