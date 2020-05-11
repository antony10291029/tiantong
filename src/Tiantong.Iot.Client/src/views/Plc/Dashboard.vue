<template>
  <div style="padding: 1.25rem">
    <PlcRunningButton :plcId="plcId" />

    <div class="columns">
      <div class="column">
        <PlcStates :plcId="plcId" />
      </div>

      <div class="column">
        <Textarea readonly style="height: 100%"/>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcRunningButton from './PlcRunningButton.vue'
import PlcStates from './PlcStates.vue'
import axios from '@/providers/axios'

@Component({
  name: 'PlcDashboard',
  components: {
    PlcStates,
    PlcRunningButton,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  isRunning = false
}
</script>
