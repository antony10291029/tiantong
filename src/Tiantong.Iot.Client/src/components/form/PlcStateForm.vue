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

      <div class="field" style="width: 320px">
        <label class="label">数据类型</label>
        <div
          @click="state.type = 'uint16'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; cursor: pointer"
        >
          <Radio :value="state.type === 'uint16'">
            uint16
          </Radio>
        </div>
        <div
          @click="state.type = 'int32'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; border-top: none;  cursor: pointer"
        >
          <Radio :value="state.type === 'int32'">
            int32
          </Radio>
        </div>
        <div
          @click="state.type = 'string'"
          class="is-bordered is-flex is-vcentered"
          style="padding: 0.5rem; border-top: none; cursor: pointer"
        >
          <Radio :value="state.type === 'string'">
            string - ASCII
          </Radio>
        </div>
      </div>

      <div
        v-if="state.type === 'string'"
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
          <Checkbox v-model="state.is_write_log_on">
            写入日志
          </Checkbox>
        </label>
      </div>
      <div class="field">
        <label class="label">
          <Checkbox v-model="state.is_read_log_on">
            读取日志
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
        v-if="state.is_collect"
      >
        <label class="label">采集间隔（毫秒）</label>
        <div class="control">
          <input
            v-model.number="state.collect_interval"
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

    <div style="padding: 1.25rem">
      <template v-if="state.is_heartbeat">
        <div
          class="field"
          style="width: 320px"
        >
          <label class="label">心跳间隔（毫秒）</label>
          <div class="control">
            <input
              v-model.number="state.heartbeat_interval"
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
              v-model.number="state.heartbeat_max_value"
              type="text" class="input"
            >
          </div>
        </div>
      </template>

      <div class="field">
        <label class="label is-flex">
          <Checkbox v-model="state.is_heartbeat">
            开启心跳
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
