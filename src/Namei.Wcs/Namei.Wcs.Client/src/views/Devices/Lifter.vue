<template>
  <section class="box">
    <p class="title is-5 is-flex" style="margin-bottom: 0.75rem">
      <span v-if="lifterId == '1'">
        改造货梯
      </span>
      <span v-else>
        提升机
      </span>

      <span class="is-flex-auto"></span>

      <span v-if="lifterId == '1'">
        #{{lifterId}}
      </span>
      <span v-else>
        #{{lifterId}}
      </span>
    </p>

    <div class="is-flex">
      <div
        class="has-background-primary is-radius"
        style="width: 40px; height: 440px; border-bottom-right-radius: 0"
      ></div>

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

export default Vue.extend({
  name: 'Lifter',

  components: {
    LifterFloor
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
  }
})
</script>
