<template>
  <div style="padding: 1.25rem;">
    <LogList search="lifter" />
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import LogList from '@/components/LogList.vue'

@Component({
  name: 'Lifters',
  components: {
    LogList
  }
})
export default class extends Vue {

}
</script>
