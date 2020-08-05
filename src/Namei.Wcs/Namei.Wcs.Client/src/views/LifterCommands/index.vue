<template>
  <div style="padding: 1.25rem; overflow: auto">
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading">
            设备名称
          </div>

          <router-link
            class="panel-block"
            to="/lifter-commands/1"
          >
            1 号梯（改造货梯）
          </router-link>
          <router-link
            class="panel-block"
            to="/lifter-commands/2"
          >
            2 号梯（提升机）
          </router-link>
          <router-link
            class="panel-block"
            to="/lifter-commands/3"
          >
            3 号梯（提升机）
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
  name: 'LifterCommands'
})
export default class extends Vue {

}
</script>
