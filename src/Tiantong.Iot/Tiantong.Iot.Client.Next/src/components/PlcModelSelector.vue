<template>
  <div class="flex flex-col border border-dark-700 rounded select-none">
    <div
      v-for="model in models" :key="model.value"
      @click="handleUpdate(model.value)"
      class="
        px-3 py-2 cursor-pointer
        border-b border-dark-700 last:border-transparent
        hover:bg-dark-800
      "
    >
      <Radio :value="value === model.value"></Radio>
      <span class="pl-2 flex-auto">{{ model.text }}</span>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { PlcModel } from "../domain";
import Radio from "../shared/Radio/index.vue";

const models = [
  {
    value: PlcModel.mc1eBinary,
    text: "三菱 MC1E - TCP（二进制）",
  },
  {
    value: PlcModel.mc3eBinary,
    text: "三菱 MC3E - TCP（二进制）",
  },
  {
    value: PlcModel.s7200Smart,
    text: "西门子 S7 - 200Smart",
  },
  {
    value: PlcModel.test,
    text: "测试协议",
  },
];

export default defineComponent({
  components: {
    Radio
  },

  props: {
    value: {
      type: String as PropType<PlcModel>,
      default: "test"
    }
  },

  emits: [
    "update:value"
  ],

  setup(props, { emit }) {
    const handleUpdate = (model: PlcModel) => {
      emit("update:value", model);
    };

    return {
      models,
      handleUpdate
    };
  }
});
</script>
