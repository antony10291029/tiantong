<template>
  <section>
    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        名称
      </label>

      <div class="is-flex-auto">

      <input
        v-model="state.name"
        type="text" class="input"
        style="width: 320px"
      >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        编号
      </label>

      <div class="is-flex-auto">

      <input
        v-model="state.number"
        type="text" class="input"
        style="width: 320px"
      >
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px">
        地址
      </label>

      <input
        v-model="state.address"
        type="text" class="input"
        style="width: 320px"
      >
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label
        class="label"
        style="width: 100px; align-self: start"
      >
        数据类型
      </label>

      <PlcStateTypes
        :type="state.type"
        :length="state.length"
        @select="handleTypeSelect"
        style="width: 320px"
      />
    </div>

    <hr>

    <template v-if="isString">
      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          字符串长度
        </label>

        <Input
          v-model.lazy="state.length"
          type="number" class="input"
          style="width: 320px"
        />
      </div>

      <hr>
    </template>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        写入日志
      </label>

      <Switcher v-model="state.is_write_log_on"></Switcher>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        读取日志
      </label>

      <Switcher v-model="state.is_read_log_on"></Switcher>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        数据采集
      </label>

      <Switcher v-model="state.is_collect"></Switcher>
    </div>

    <hr>

    <template v-if="state.is_collect">
      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          采集间隔
        </label>

        <input
          v-model.lazy.number="state.collect_interval"
          type="number" class="input"
          style="width: 320px"
        >
        <label class="label">（毫秒）</label>
      </div>

      <hr>
    </template>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        心跳写入
      </label>

      <Switcher v-model="state.is_heartbeat"></Switcher>
    </div>

    <hr>

    <template v-if="state.is_heartbeat">
      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          心跳间隔
        </label>

        <input
          v-model.number="state.heartbeat_interval"
          type="text" class="input"
          style="width: 320px"
        >
        <label class="label">（毫秒）</label>
      </div>

      <hr>
      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 100px">
          心跳最大值
        </label>

        <input
          v-model.number="state.heartbeat_max_value"
          type="text" class="input"
          style="width: 320px"
        >
      </div>

      <hr>
    </template>
  </section>
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
