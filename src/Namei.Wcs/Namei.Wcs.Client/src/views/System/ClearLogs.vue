<template>
  <div
    class="is-flex is-vcentered"
    style="padding: 1.25rem 0"
  >
    <p class="label" style="width: 180px">
      清理日志
    </p>

    <label
      class="label"
      @click="days = 30"
    >
      <Radio :value="days === 30">
        保留 30 天
      </Radio>
    </label>

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 15"
    >
      <Radio :value="days === 15">
        保留 15 天
      </Radio>
    </label>

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 7"
    >
      <Radio :value="days === 7">
        保留 7 天
      </Radio>
    </label>

    <div style="width: 1.5rem"></div>

    <a class="button is-info">
      开始清理
    </a>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'ClearLogs'
})
export default class extends Vue {
  days = 30


}
</script>
