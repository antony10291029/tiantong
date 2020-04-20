<template>
  <div class="container">
    test
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Comp from './Component.vue'

@Component({
  name: 'test'
})
export default class extends Vue {

}
</script>
