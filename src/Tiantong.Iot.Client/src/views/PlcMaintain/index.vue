<template>
  <div style="padding: 1.25rem">
    <div class="is-bordered">
      <ClearLogs></ClearLogs>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import ClearLogs from './ClearLogs.vue'

@Component({
  name: 'PlcMaintain',
  components: {
    ClearLogs
  }
})
export default class extends Vue {

}
</script>
