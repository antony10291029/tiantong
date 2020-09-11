<template>
  <div style="padding: 1.25rem; overflow: auto">
    <LogList search="door" />
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
