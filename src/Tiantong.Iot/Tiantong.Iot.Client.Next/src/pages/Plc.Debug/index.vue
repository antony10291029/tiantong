<template>
  <div class="p-4 text-md last:mb-0 flex-auto h-full overflow-y-auto sm:flex sm:flex-col">
    <p class="text-2xl mt-2">
      实时调试
    </p>

    <hr>

    <div class="sm:flex gap-4 sm:flex-auto">
      <div class="w-full sm:max-w-min">
        <div class="flex items-center">
          <p class="whitespace-nowrap w-28">操作</p>

          <div class="w-full sm:w-72">
            <TheCommand v-model:value="params.command" />
          </div>
        </div>

        <hr>

        <div class="flex items-start">
          <p class="whitespace-nowrap w-28">数据类型</p>

          <div class="w-full sm:w-72">
            <PlcStateTypeSelector
              v-model:value="params.dataType"
              :length="params.length"
            />
          </div>
        </div>

        <hr>

        <template v-if="isString">
          <div class="flex items-center">
            <p class="whitespace-nowrap w-28">字符串长度</p>
            <div class="w-full sm:w-72">
              <Input value="4"/>
            </div>
          </div>

          <hr>
        </template>

        <div class="flex items-center">
          <p class="whitespace-nowrap w-28">地址</p>
          <div class="w-full sm:w-72">
            <Input v-model:value="params.address" />
          </div>
        </div>

        <hr>

        <template v-if="!isGet">
          <div class="flex items-center">
            <p class="whitespace-nowrap w-28">数据</p>
            <div class="w-full sm:w-72">
              <Input v-model:value="params.value"/>
            </div>
          </div>

          <hr>
        </template>

        <div class="flex items-center">
          <p class="whitespace-nowrap w-28"></p>

          <div class="w-full sm:w-72">
            <Button @click="handleSubmit">
              执行
            </Button>
          </div>
        </div>
      </div>

      <hr>

      <div class="w-full h-80 sm:h-full sm:flex-auto">
        <Textarea
          :value="messages.join('\n')"
          class="h-full"
          readonly
        />
      </div>
    </div>

  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import { PlcStateCommand, PlcStateType, PlcWorkerContext, DebugParams } from "../../domain";
import { DateTime } from "../../shared/data-time";
import Input from "../../shared/components/Form.Input/index.vue";
import Textarea from "../../shared/components/Form.Textarea/index.vue";
import Button from "../../shared/components/Button/index.vue";
import PlcStateTypeSelector from "../../components/PlcStateTypeSelector.vue";
import TheCommand from "./TheCommand.vue";

export default defineComponent({
  components: {
    Input,
    Button,
    Textarea,
    TheCommand,
    PlcStateTypeSelector,
  },

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const params = ref<DebugParams>({
      plcId: props.plcId,
      command: PlcStateCommand.get,
      dataType: PlcStateType.uint16,
      length: 4,
      address: "D100",
      value: "0"
    });
    const messages = ref<string[]>([]);
    const isGet = computed(() => params.value.command === PlcStateCommand.get);
    const isString = computed(() => params.value.dataType === PlcStateType.string);
    const handleSubmit = async () => {
      const result = await PlcWorkerContext.debug(params.value);
      const time = DateTime.now.split("T")[1].split(".")[0];

      messages.value.unshift(`[${time}] ${result.data.message}`);

      if (messages.value.length > 50) {
        messages.value.splice(50, messages.value.length - 50);
      }
    };

    return {
      params,
      messages,
      isGet,
      isString,
      handleSubmit,
    };
  }
});
</script>
