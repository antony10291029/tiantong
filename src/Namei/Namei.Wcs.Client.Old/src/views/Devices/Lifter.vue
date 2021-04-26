<template>
  <section class="box">
    <div class="title is-5 is-flex is-vcentered" style="margin-bottom: 0.75rem">
      <span v-if="lifterId == '1'">
        {{lifterId}}# 改造货梯
      </span>
      <span v-else>
        {{lifterId}}# 提升机
      </span>
    </div>

    <div class="is-flex">
      <div
        class="has-background-primary is-radius is-flex is-flex-column is-centered is-vcentered"
        style="width: 84px; height: 440px; border-bottom-right-radius: 0; padding: 0.5rem"
      >
        <span
          v-if="isAlerting"
          class="tag is-danger is-light"
          style="width: 100%"
        >
          报警中
        </span>

        <template v-if="isTransformedLifter">
          <div style="height: 0.5rem"></div>

          <PalletCode
            :code="lifter.palletCodeB"
            style="width: 100%"
          />
        </template>

        <div style="height: 0.5rem"></div>

        <PalletCode
          :code="lifter.palletCodeA"
          style="width: 100%"
        />
      </div>

      <!-- 输送机 -->
      <div>
        <LifterFloor
          v-for="floor in [4, 3, 2, 1]" :key="floor"
          :floor="floor"
          :lifterId="lifterId"
          :door="doors[`${floor}5${lifterId}`]"
          :floorState="lifter.floors[floor - 1]"
        />
      </div>
    </div>
  </section>
</template>

<script>
import Vue from 'vue'
import LifterFloor from './LifterFloor.vue'
import LifterPlatform from './LifterPlatform.vue'
import PalletCode from './PalletCode.vue'

export default Vue.extend({
  name: 'Lifter',

  components: {
    LifterFloor,
    LifterPlatform,
    PalletCode,
  },

  props: {
    lifterId: {
      type: String,
      required: true
    },
    lifter: {
      type: Object,
      required: true
    },
    doors: {
      type: Object,
      required: true
    }
  },

  computed: {
    isAlerting () {
      return this.lifter.isAlerting
    },

    isTransformedLifter () {
      return this.lifterId === '1'
    }
  }
})
</script>
