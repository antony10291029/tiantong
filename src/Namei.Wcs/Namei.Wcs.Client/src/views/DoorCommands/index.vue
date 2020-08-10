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
            自动门 - 101
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/102"
          >
            自动门 - 102
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/103"
          >
            自动门 - 103
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/201"
          >
            自动门 - 201
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/202"
          >
            自动门 - 202
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/901"
          >
            自动门 - 901
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/902"
          >
            自动门 - 902
          </router-link>
          <router-link
            class="panel-block"
            to="/door-commands/903"
          >
            自动门 - 903
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
