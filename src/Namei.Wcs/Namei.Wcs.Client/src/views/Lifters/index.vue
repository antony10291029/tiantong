<template>
  <div
    class="is-flex is-flex-column"
    style="overflow: auto;"
  >
    <div
      class="tabs"
      style="margin-bottom: 0; font-size: 1.125rem; flex-shrink: 0"
    >
      <ul>
        <li class="is-active">
          <a>设备状态</a>
        </li>
      </ul>
    </div>
    <div
      class="is-flex-auto"
      style="padding: 1.25rem;"
    >
      <div class="columns">
        <div class="column">
          <div class="box">
            <LifterStates
              lifterId="1"
              text="改造货梯"
            />

            <div style="height: 1.25rem"></div>

            <LifterStates
              lifterId="2"
              text="提升机 - 1"
            />

            <div style="height: 1.25rem"></div>

            <LifterStates
              lifterId="3"
              text="提升机 - 2"
            />
          </div>
        </div>

        <div class="column">
          <LogList search="lifter" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import LogList from '@/components/LogList.vue'
import LifterStates from './LifterStates.vue'

@Component({
  name: 'Lifters',
  components: {
    LogList,
    LifterStates
  }
})
export default class extends Vue {

}
</script>
