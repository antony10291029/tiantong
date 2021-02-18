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

      <Input
        v-model:value="state.name"
        type="text"
        style="width: 320px"
      />
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

      <Input
        v-model:value="state.number"
        type="text"
        style="width: 320px"
      />
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

      <Input
        v-model:value="state.address"
        type="text" class="input"
        style="width: 320px"
      />
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
        v-model:value="state.type"
        v-model:length="state.length"
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
          v-model:value="state.length"
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

      <Switcher v-model:value="state.is_write_log_on"/>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        读取日志
      </label>

      <Switcher v-model:value="state.is_read_log_on"/>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        数据采集
      </label>

      <Switcher v-model:value="state.is_collect"/>
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

        <Input
          v-model:value="state.collect_interval"
          type="number"
          style="width: 320px"
        />
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

      <Switcher v-model:value="state.is_heartbeat"/>
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

        <Input
          v-model:value="state.heartbeat_interval"
          type="number"
          style="width: 320px"
        />
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

        <Input
          v-model:value="state.heartbeat_max_value"
          type="number"
          style="width: 320px"
        />
      </div>

      <hr>
    </template>
  </section>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from "vue";
import { PlcState, PlcStateType } from "@/entities";
import PlcStateTypes from "@/components/PlcStateTypes.vue";

export default defineComponent({
  name: "PlcStateForm",

  components: {
    PlcStateTypes,
  },

  props: {
    state: {
      type: Object as PropType<PlcState>,
      required: true
    },

    handler: {
      type: Function,
      default: () => () => {}
    },
  },

  setup(props) {
    const isBool = computed(() => props.state.type === PlcStateType.bool);
    const isString = computed(() => props.state.type === PlcStateType.string);

    function handleTypeSelect(type: string) {
      props.state.type = type;
      if (isBool.value) {
        props.state.is_heartbeat = false;
      }
    }

    return {
      isBool,
      isString,
      handleTypeSelect
    };
  }
});
</script>
