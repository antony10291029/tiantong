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
            to="/door-commands/101"
          >
            101 号自动门
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/102"
          >
            102 号自动门
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/103"
          >
            103 号自动门
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/201"
          >
            201 号自动门
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/202"
          >
            202 号自动门
          </router-link>
        </div>
      </div>

      <div class="column">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'DoorCommands'
})
export default class extends Vue {

}
</script>
