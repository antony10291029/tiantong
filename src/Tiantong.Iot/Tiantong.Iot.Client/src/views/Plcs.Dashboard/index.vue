<template>
  <div style="padding: 1.25rem">
    <div class="box is-paddingless">
      <div
        style="border-bottom: none"
      >
        <QuickOperations />
      </div>

      <hr style="margin: 1.25rem">

      <div>
        <Autorun />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import QuickOperations from './QuickOperations.vue'
import Autorun from './Autorun.vue'

@Component({
  name: 'PlcsDashboard',
  components: {
    Autorun,
    QuickOperations
  }
})
export default class extends Vue {

}
</script>
