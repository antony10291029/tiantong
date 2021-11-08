<template>
  <div class="p-4 text-md last:mb-0 flex-auto h-full overflow-y-auto">
    <p class="text-2xl mt-2">
      配置管理
    </p>

    <hr>

    <div class="field">
      <label class="w-28">
        设备名称
      </label>
      <div class="w-80">
        <Input v-model:value="params.name" />
      </div>
    </div>

    <hr>

    <div class="field">
      <label class="w-28">
        设备编号
      </label>
      <div class="w-80">
        <Input v-model:value="params.number" />
      </div>
    </div>

    <hr>

    <div class="field items-start">
      <label class="w-28 self-start">
        通信协议
      </label>
      <div class="w-80">
        <PlcModelSelector v-model:value="params.model" />
      </div>
    </div>

    <hr>

    <div class="field">
      <label class="w-28">
        IP 地址
      </label>
      <div class="w-80">
        <Input v-model:value="params.host" />
      </div>
    </div>

    <hr>

    <div class="field">
      <label class="w-28">
        IP 端口
      </label>
      <div class="w-80">
        <InputNumber v-model:value="params.port" />
      </div>
    </div>

    <hr>

    <div class="field">
      <label class="self-start w-28">
        备注
      </label>
      <div class="w-96">
        <Textarea v-model:value="params.comment"></Textarea>
      </div>
    </div>

    <hr>

    <div class="field">
      <label class="w-28"></label>

      <div class="w-96">
        <Button @click="handleSubmit">
          保存
        </Button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, PropType } from "vue";
import { PlcConfig, PlcConfigContext } from "../../domain";
import Input from "../../shared/components/Form.Input/index.vue";
import Textarea from "../../shared/components/Form.Textarea/index.vue";
import InputNumber from "../../shared/components/Form.InputNumber/index.vue";
import PlcModelSelector from "../../components/PlcModelSelector.vue";
import Button from "../../shared/components/Button/index.vue";

export default defineComponent({
  components: {
    Button,
    PlcModelSelector,
    Input,
    Textarea,
    InputNumber,
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
