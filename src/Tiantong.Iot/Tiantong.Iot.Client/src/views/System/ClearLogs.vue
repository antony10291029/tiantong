<template>
  <div style="padding: 1.25rem">
    <p class="title is-5">
      日志清理
    </p>

    <p class="content">
      日志数据量过大将影响系统性能，若有需要可根据以下条件清理进行清理。
    </p>

    <div class="is-flex">
      <label
        class="label"
        @click="days = 30"
      >
        <Radio :value="days === 30"></Radio>
        保留 30 天
      </label>

      <div style="width: 1rem"></div>

      <label
        class="label"
        @click="days = 7"
      >
        <Radio :value="days === 7"></Radio>
        保留 7 天
      </label>

      <div style="width: 1rem"></div>

      <label
        class="label"
        @click="days = 0"
      >
        <Radio :value="days === 0"></Radio>
        清理全部
      </label>
    </div>

    <div style="height: 0.75rem"></div>

    <div class="is-flex is-vcentered">
      <AsyncButton
        :handler="handleDelete"
        class="button is-info is-light is-small"
      >
        开始清理
      </AsyncButton>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'ClearLogs'
})
export default class extends Vue {
  days = 30

  handleDelete () {
    this.$confirm({
      title: '提示',
      content: '该操作将删除日志，请确认',
      handler: async () => await axios.post('/plc-logs/clear', { days: this.days })
    })
  }
}
</script>
