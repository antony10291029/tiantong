<template>
  <div style="padding: 1.25rem; overflow: auto">
    <div class="columns">
      <div class="column is-narrow">
        <TheStates />
      </div>

      <div class="column">
        <LogList search="door" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import TheStates from './States.vue'
import LogList from '@/components/LogList.vue'

@Component({
  name: 'AutoDoors',
  components: {
    LogList,
    TheStates,
  }
})
export default class extends Vue {

}
</script>
