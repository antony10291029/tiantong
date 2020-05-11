<template>
  <div class="field has-addons">
    <p class="control">
      <a
        v-if="!isRunning"
        @click="isRunning = !isRunning"
        class="button is-small is-info"
      >
        <span>运行</span>
        <span class="icon is-small">
          <i class="iconfont icon-play"></i>
        </span>
      </a>
      <a
        v-else
        @click="isRunning = !isRunning"
        class="button is-small is-info is-light"
      >
        <span>停止</span>
        <span class="icon is-small">
          <i class="iconfont icon-stop"></i>
        </span>
      </a>
    </p>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'PlcRunningButton'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId = 0

  isRunning = false
}
</script>
