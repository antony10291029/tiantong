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

    <div style="width: 1rem"></div>

    <label
      class="label"
      @click="days = 0"
    >
      <Radio :value="days === 0">
        全部清理
      </Radio>
    </label>

    <div style="width: 1.5rem"></div>

    <AsyncButton
      :handler="handleClick"
      class="button is-info"
    >
      开始清理
    </AsyncButton>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'ClearLogs'
})
export default class extends Vue {
  days = 30

  handleClick () {
    console.log(this.days)

    this.$confirm({
      title: '确认',
      content: '清除日志后将无法找回',
      handler: () => domain.post('/logs/clear', { days: this.days })
    })
  }
}
</script>
