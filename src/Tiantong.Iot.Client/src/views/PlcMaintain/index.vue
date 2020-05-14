<template>
  <div style="padding: 1.25rem">
    <div class="is-bordered">
      <ClearLogs></ClearLogs>
    </div>

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <SetPassword></SetPassword>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import ClearLogs from './ClearLogs.vue'
import SetPassword from './SetPassword.vue'

@Component({
  name: 'PlcMaintain',
  components: {
    ClearLogs,
    SetPassword
  }
})
export default class extends Vue {

}
</script>
