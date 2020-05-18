<template>
  <div style="padding: 1.25rem">
    <div class="is-bordered">
      <ClearLogs></ClearLogs>
    </div>

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <SystemLock></SystemLock>
    </div>

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <Autorun></Autorun>
    </div>

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <StopDevices></StopDevices>
    </div>

    <div
      class="is-bordered"
      style="border-top: none"
    >
      <AdminEmail></AdminEmail>
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
import Autorun from './Autorun.vue'
import ClearLogs from './ClearLogs.vue'
import AdminEmail from './AdminEmail.vue'
import SystemLock from './SystemLock.vue'
import SetPassword from './SetPassword.vue'
import StopDevices from './StopDevices.vue'

@Component({
  name: 'PlcMaintain',
  components: {
    Autorun,
    ClearLogs,
    AdminEmail,
    SystemLock,
    SetPassword,
    StopDevices
  }
})
export default class extends Vue {

}
</script>
