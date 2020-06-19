<template>
  <div
    class="is-bordered"
    style="border-top: none; border-bottom: none"
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

        <PlcStateTypes
          :type="state.type"
          :length="state.length"
          @select="handleTypeSelect"
        />
      </div>

      <div
        v-if="isString"
        class="field" style="width: 320px"
      >
        <label class="label">字符串长度</label>
        <div class="control">
          <input
            v-model.lazy.number="state.length"
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

    <div
      v-if="!isBool"
      class="has-border-bottom"
      style="padding: 1.25rem;"
    >
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
import { PlcState, PlcStateType } from '@/entities'
import PlcStateTypes from '@/components/PlcStateTypes.vue'

@Component({
  name: 'PlcStateForm',
  components: {
    PlcStateTypes,
  }
})
export default class extends Vue {
  @Prop({ required: true })
  state!: PlcState

  @Prop({ default: () => () => {} })
  handler!: () => {}

  get isBool () {
    return this.state.type === PlcStateType.bool
  }

  get isString () {
    return this.state.type === PlcStateType.string
  }

  handleTypeSelect (type: string) {
    this.state.type = type
    if (this.isBool) {
      this.state.is_heartbeat = false;
    }
  }
}
</script>
