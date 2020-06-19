<template>
  <div
    class="is-flex-auto has-background-white"
    style="overflow: auto"
  >
    <section
      class="container is-flex-auto"
      style="overflow: auto; max-width: 768px; margin: 1.5rem auto 1.5rem auto"
    >
      <div
        v-if="false"
        class="is-bordered"
      >
        <AdminEmail></AdminEmail>
      </div>

      <div
        class="is-bordered"
      >
        <SystemLock></SystemLock>
      </div>

      <div
        class="is-bordered"
        style="border-top: none"
      >
        <ClearLogs></ClearLogs>
      </div>

      <div
        class="is-bordered"
        style="border-top: none"
      >
        <SetPassword></SetPassword>
      </div>
    </section>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import ClearLogs from './ClearLogs.vue'
import AdminEmail from './AdminEmail.vue'
import SystemLock from './SystemLock.vue'
import SetPassword from './SetPassword.vue'

@Component({
  name: 'PlcMaintain',
  components: {
    ClearLogs,
    AdminEmail,
    SystemLock,
    SetPassword
  }
})
export default class extends Vue {

}
</script>
