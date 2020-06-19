<template>
  <span class="icon">
    <i v-if="value" class="iconfont has-text-success icon-success"></i>
    <i v-else class="iconfont has-text-danger icon-danger"></i>
  </span>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'YesOrNoCell'
})
export default class extends Vue {
  @Prop({ required: true })
  value!: Boolean
}
</script>
