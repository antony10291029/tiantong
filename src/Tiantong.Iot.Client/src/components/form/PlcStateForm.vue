<template>
  <div
    class="is-bordered"
    style="border-top: none"
  >
    <div
      class="has-border-bottom"
      style="padding: 1.25rem"
    >
      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">名称</label>
        <div class="control">
          <input
            v-model="state.name"
            type="text" class="input"
          >
        </div>
      </div>

      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">地址</label>
        <div class="control">
          <input
            v-model="state.address"
            type="text" class="input"
          >
        </div>
      </div>

      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">数据类型</label>
        <div class="control">
          <input
            v-model="state.type"
            type="text" class="input"
          >
        </div>
      </div>

      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">数据长度</label>
        <div class="control">
          <input
            v-model.number="state.length"
            type="number" class="input"
          >
        </div>
      </div>

      <div class="field">
        <label class="label">
          <Checkbox v-model="state.open_log">
            读写日志
          </Checkbox>
        </label>
      </div>
    </div>

    <div
      class="has-border-bottom"
      style="padding: 1.25rem"
    >
      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">心跳频率</label>
        <div class="control">
          <input
            v-model="state.heartbeat_interval"
            type="text" class="input"
          >
        </div>
      </div>

      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">心跳最大值</label>
        <div class="control">
          <input
            v-model="state.heartbeat_max_value"
            type="text" class="input"
          >
        </div>
      </div>

      <div class="field">
        <label class="label is-flex">
          <Checkbox v-model="state.is_heartbeat">
            开启心跳
          </Checkbox>
        </label>
      </div>
    </div>

    <div style="padding: 1.25rem">
      <div
        class="field"
        style="width: 320px"
      >
        <label class="label">采集频率</label>
        <div class="control">
          <input
            v-model="state.collect_interval"
            type="text" class="input"
          >
        </div>
      </div>

      <div class="field">
        <label class="label is-flex">
          <Checkbox v-model="state.is_collect">
            开启数据采集
          </Checkbox>
        </label>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { PlcState } from '@/entities'

@Component({
  name: 'PlcStateForm'
})
export default class extends Vue {
  @Prop({ required: true })
  state!: PlcState

  @Prop({ default: () => () => {} })
  handler!: () => {}
}
</script>
