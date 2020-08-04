<template>
  <div style="padding: 1.25rem">
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading">
            设备名称
          </div>

          <router-link
            class="panel-block"
            to="/lifters-debug/1"
          >
            改造货梯（1 号梯）
          </router-link>
          <router-link
            class="panel-block"
            to="/lifters-debug/2"
          >
            提升机 1（2 号梯）
          </router-link>
          <router-link
            class="panel-block"
            to="/lifters-debug/3"
          >
            提升机 2（3 号梯）
          </router-link>
        </div>
      </div>

      <div class="column">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'LiftersDebug'
})
export default class extends Vue {

}
</script>
