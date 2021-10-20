<template>
  <div class="flex flex-col border border-dark-700 rounded select-none">
    <div
      v-for="model in models" :key="model.value"
      @click="handleUpdate(model.value)"
      class="
        px-3 py-1.5 cursor-pointer group
        border-b border-dark-700
        hover:bg-dark-800
        first:rounded-t last:border-0 last:rounded-b
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
import Radio from "../shared/components/Radio/index.vue";

type SelectOption = {
  value: PlcModel,
  text: string,
};

const models: Array<SelectOption> = [
  {
    value: "mc1e-binary",
    text: "三菱 MC1E - TCP（二进制）",
  },
  {
    value: "mc3e-binary",
    text: "三菱 MC3E - TCP（二进制）",
  },
  {
    value: "s7200smart",
    text: "西门子 S7 - 200Smart",
  },
  {
    value: "test",
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
