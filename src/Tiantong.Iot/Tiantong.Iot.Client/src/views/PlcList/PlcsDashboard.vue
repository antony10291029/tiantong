<template>
  <div style="padding: 1.25rem">
    <div
      class="is-bordered"
      style="border-bottom: none"
    >
      <QuickOperations />
    </div>
    <div
      class="is-bordered"
    >
      <Autorun />
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
