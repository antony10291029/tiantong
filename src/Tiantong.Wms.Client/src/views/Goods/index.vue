<template>
  <router-view v-bind="$attrs"></router-view>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'Stocks'
})
export default class extends Vue {

}
</script>
