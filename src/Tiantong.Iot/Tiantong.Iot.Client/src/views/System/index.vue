<template>
  <div
    class="is-flex-auto"
    style="overflow: auto"
  >
    <section
      class="container is-flex-auto box is-paddingless"
      style="overflow: auto; max-width: 768px; margin: 1.5rem auto 1.5rem auto"
    >
      <div>
        <Account
          :isLogged="isLogged"
          @logout="isLogged = $event">
        </Account>
      </div>

      <hr>

      <template v-if="isLogged">
        <div>
          <SystemLock :isLogged="isLogged"></SystemLock>
        </div>

        <hr>
      </template>

      <div style="border-top: none">
        <ClearLogs></ClearLogs>
      </div>

    </section>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Account from './Account.vue'
import ClearLogs from './ClearLogs.vue'
import SystemLock from './SystemLock.vue'
import SetPassword from './SetPassword.vue'
import { account as store } from '@/providers/local-storage'

@Component({
  name: 'PlcMaintain',
  components: {
    Account,
    ClearLogs,
    SystemLock
  }
})
export default class extends Vue {
  isLogged = store.token.get()
}
</script>
