<template>
  <div class="p-4 flex flex-col text-md last:mb-0 flex-auto h-full overflow-y-auto">
    <p class="text-2xl mt-2">
      配置管理
    </p>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-center flex-shrink">
      <p class="whitespace-nowrap w-28">设备名称</p>
      <div class="w-full sm:w-80">
        <TheInput v-model:value="params.name" />
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-center flex-shrink">
      <p class="whitespace-nowrap w-28">设备编号</p>
      <div class="w-full sm:w-80">
        <TheInput v-model:value="params.number" />
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex">
      <p class="whitespace-nowrap w-28">通信协议</p>
      <div class="w-full sm:w-80">
        <PlcModelSelector v-model:value="params.model" />
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-center">
      <p class="whitespace-nowrap w-28">IP 地址</p>
      <div class="w-full sm:w-80">
        <TheInput v-model:value="params.host" />
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-center">
      <p class="whitespace-nowrap w-28">IP 端口</p>
      <div class="w-full sm:w-80">
        <TheInputNumber v-model:value="params.port" />
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-start">
      <p class="whitespace-nowrap w-28">备注</p>
      <div class="w-full sm:w-96">
        <TheTextarea v-model:value="params.comment"/>
      </div>
    </div>

    <hr class="border-0 h-0.5 rounded-full bg-dark-800 my-6">

    <div class="flex items-center">
      <p class="whitespace-nowrap w-28"></p>

      <div class="w-full sm:w-96">
        <button
          @click="handleSubmit"
          class="
            cursor-pointer z-50
            flex px-4 py-1.5
            items-center justify-center
            ring-2 ring-transparent bg-info-600
            hover:bg-info-700
            active:bg-info-800
            disabled:opacity-50 disabled:cursor-not-allowed
          "
        >
          保存
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, PropType } from "vue";
import { PlcConfig, PlcConfigContext } from "../../domain";
import TheInput from "./TheInput.vue";
import TheTextarea from "./TheTextarea.vue";
import TheInputNumber from "./TheInputNumber.vue";
import PlcModelSelector from "../../components/PlcModelSelector.vue";

export default defineComponent({
  components: {
    TheInput,
    TheTextarea,
    TheInputNumber,
    PlcModelSelector
  },

  props: {
    plcConfig: {
      type: Object as PropType<PlcConfig>,
      required: true,
    }
  },

  emits: [
    "updated"
  ],

  setup(props, { emit }) {
    const params = ref<PlcConfig>({ ...props.plcConfig });
    const handleSubmit = async () => {
      await PlcConfigContext.update(params.value);
      emit("updated");
    };

    return {
      params,
      handleSubmit
    };
  }
});
</script>
